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

using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content.Workflow.Exceptions;
using DotNetNuke.Entities.Content.Workflow.Repositories;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Entities.Content.Workflow
{
    public class WorkflowManager : ServiceLocator<IWorkflowManager, WorkflowManager>, IWorkflowManager
    {
        private readonly DataProvider _dataProvider;
        private readonly IWorkflowRepository _workflowRepository = WorkflowRepository.Instance;
        private readonly IWorkflowStateRepository _workflowStateRepository = WorkflowStateRepository.Instance;
        private readonly ISystemWorkflowManager _systemWorkflowManager = SystemWorkflowManager.Instance;

        #region Constructor
        public WorkflowManager()
        {
            _dataProvider = DataProvider.Instance();
        }
        #endregion

        #region Public Methods

        public void DeleteWorkflow(ContentWorkflow workflow)
        {
            if (workflow.IsSystem)
            {
                throw new WorkflowException(Localization.GetString("SystemWorkflowDeletionException", Localization.ExceptionsResourceFile));
            }

            var usageCount = GetWorkflowUsageCount(workflow.WorkflowID);
            if (usageCount > 0)
            {
                throw new WorkflowException(Localization.GetString("WorkflowInUsageException", Localization.ExceptionsResourceFile));
            }

            _workflowRepository.DeleteWorkflow(workflow);
        }

        public ContentWorkflow GetWorkflow(int workflowId)
        {
            return _workflowRepository.GetWorkflow(workflowId);
        }

        public ContentWorkflow GetWorkflow(ContentItem item)
        {
            if (item.StateID == Null.NullInteger)
            {
                return null;
            }
            var state = WorkflowStateRepository.Instance.GetWorkflowStateByID(item.StateID);
            return state == null ? null : GetWorkflow(state.WorkflowID);
        }

        public ContentWorkflow GetCurrentOrDefaultWorkflow(ContentItem item, int portalId)
        {
            if (item.StateID != Null.NullInteger)
            {
                return GetWorkflow(item);
            }
               
            var defaultWorkflow = WorkflowSettings.Instance.GetDefaultTabWorkflowId(portalId);
            return GetWorkflow(defaultWorkflow);
        }

        public IEnumerable<ContentWorkflow> GetWorkflows(int portalId)
        {
            return _workflowRepository.GetWorkflows(portalId);
        }

        public void AddWorkflow(ContentWorkflow workflow)
        {
            _workflowRepository.AddWorkflow(workflow);

            var firstDefaultState = _systemWorkflowManager.GetDraftStateDefinition(1);
            var lastDefaultState = _systemWorkflowManager.GetPublishedStateDefinition(2);

            firstDefaultState.WorkflowID = workflow.WorkflowID;
            lastDefaultState.WorkflowID = workflow.WorkflowID;

            _workflowStateRepository.AddWorkflowState(firstDefaultState);
            _workflowStateRepository.AddWorkflowState(lastDefaultState);

            workflow.States = new List<ContentWorkflowState>
                              {
                                  firstDefaultState,
                                  lastDefaultState
                              };
        }

        public void UpdateWorkflow(ContentWorkflow workflow)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ContentItem> GetWorkflowUsage(int workflowId, int pageIndex, int pageSize)
        {
            return CBO.FillCollection<ContentItem>(_dataProvider.GetContentWorkflowUsage(workflowId, pageIndex, pageSize));
        }

        public int GetWorkflowUsageCount(int workflowId)
        {
            return _dataProvider.GetContentWorkflowUsageCount(workflowId);
        }
        #endregion

        protected override System.Func<IWorkflowManager> GetFactory()
        {
            return () => new WorkflowManager();
        }
    }
}
