﻿#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Entities.Content.Workflow.Exceptions;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Entities.Content.Workflow
{
    // TODO: add interface metadata documentation
    // TODO: removed unused SPRoc and DataProvider layer
    internal class WorkflowController : ServiceLocator<IWorkflowController, WorkflowController>, IWorkflowController
    {
        private readonly IWorkflowStateController _stateController = WorkflowStateController.Instance;

        #region Constructor
        public WorkflowController()
        {
            
        }

        public WorkflowController(IWorkflowStateController stateController)
        {
            _stateController = stateController;
        }
        #endregion

        public IEnumerable<ContentWorkflow> GetWorkflows(int portalId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();
                return rep.Find("WHERE IsDeleted = 0 AND (PortalId = @0 OR PortalId IS NULL)", portalId);
            }
        }

        public IEnumerable<ContentWorkflow> GetSystemWorkflows(int portalId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();
                return rep.Find("WHERE IsDeleted = 0 AND (PortalId = @0 OR PortalId IS NULL) AND IsSystem = 1", portalId);
            }
        }

        public ContentWorkflow GetWorkflowByID(int workflowId)
        {
            ContentWorkflow workflow;
            using(var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();
                workflow = rep.Find("WHERE WorkflowId = @0 AND IsDeleted = 0", workflowId).SingleOrDefault();
            }
            
            if (workflow == null)
            {
                return null;
            }

            workflow.States = _stateController.GetWorkflowStates(workflowId);
            return workflow;
        }

        public ContentWorkflow GetWorkflow(ContentItem item)
        {
            var state = _stateController.GetWorkflowStateByID(item.StateID);
            return state == null ? null : GetWorkflowByID(state.WorkflowID);
        }

        // TODO: validation
        public void AddWorkflow(ContentWorkflow workflow)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();

                if (DoesExistWorkflow(workflow, rep))
                {
                    throw new WorkflowNameAlreadyExistsException();
                }
                rep.Insert(workflow);
            }
        }
        
        // TODO: validation
        public void UpdateWorkflow(ContentWorkflow workflow)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();

                if (DoesExistWorkflow(workflow, rep))
                {
                    throw new WorkflowNameAlreadyExistsException();
                }
                rep.Update(workflow);
            }
        }

        // Todo: workflow cannot be deleted if in usage
        public void DeleteWorkflow(ContentWorkflow workflow)
        {
            var usageCount = DataProvider.Instance().GetContentWorkflowUsageCount(workflow.WorkflowID);
            if (usageCount > 0)
            {
                throw new WorkflowException(Localization.GetString("WorkflowInUsageException", Localization.ExceptionsResourceFile));    
            }

            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ContentWorkflow>();
                rep.Update("SET IsDeleted = 1 WHERE IsDeleted = 0 AND WorkflowId = @0", workflow.WorkflowID);
            }
        }

        protected override Func<IWorkflowController> GetFactory()
        {
            return () => new WorkflowController();
        }

        #region Private Methods

        private static bool DoesExistWorkflow(ContentWorkflow workflow, IRepository<ContentWorkflow> rep)
        {
            return rep.Find(
                "WHERE IsDeleted = 0 AND (PortalId = @0 OR PortalId IS NULL) AND WorkflowName = @1 AND WorkflowID != @2",
                workflow.PortalID, workflow.WorkflowName, workflow.WorkflowID).SingleOrDefault() != null;
        }
        #endregion
    }
}