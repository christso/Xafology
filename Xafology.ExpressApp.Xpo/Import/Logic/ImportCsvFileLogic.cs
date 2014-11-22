using Xafology.Utils;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Xafology.ExpressApp.Xpo.Import
{
    public delegate void ImportCsvFileEventHandler();

    public abstract class ImportCsvFileLogic
    {
        public event ImportCsvFileEventHandler BeforeImport;
        public event ImportCsvFileEventHandler AfterImport;

        public ImportErrorInfo ErrorInfo { get; set; }
        protected readonly ImportCsvFileParamBase _Param;
        protected readonly XpoImportEngine _ImportEngine;
        protected CsvReader _csvReader;
        private XafApplication _Application;

        protected XafApplication Application
        {
            get
            {
                return _Application;
            }
        }


        protected void OnBeforeImport()
        {
            if (BeforeImport != null)
                BeforeImport();
        }

        protected void OnAfterImport()
        {
            if (AfterImport != null)
                AfterImport();
        }

        /// <summary>
        /// Type Info for the object to import data to
        /// </summary>
        protected ITypeInfo _objTypeInfo;

        /// <summary>
        /// Assign this to the RequestManager.CancellationTokenSource.
        /// If the user sets the cancellation IsCancellationRequested to true,
        /// then the import will stop running.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public ImportCsvFileLogic(XafApplication application, ImportCsvFileParamBase param)
        {
            _Param = param;
            ErrorInfo = null;
            CancellationTokenSource = null;
            _ImportEngine = new XpoImportEngine(application);
            Options = new CsvImportOptions(this);
            _objTypeInfo = param.ObjectTypeInfo;
            _Application = application;
        }

        public abstract void Import();

        /// <summary>
        /// Create Field Import Maps required to import into XPO assuming
        /// that the CSV field names (source) are the same as the XPO field names (target).
        /// Note: This will not delete existing maps. It will only add maps which means
        /// it may result in duplicate maps and the import failing as a result.
        /// </summary> 
        /// <param name="stream">Import File stream</param>
        public abstract void CreateFieldImportMaps();

        /// <summary>
        /// Inserts CSV data from the stream into data store
        /// </summary>
        public abstract void Insert();

        /// <summary>
        /// Updates the data store with CSV data
        /// </summary>
        public abstract void Update();

        public Dictionary<Type, List<string>> XpObjectsNotFound
        {
            get
            {
                return _ImportEngine.XpObjectsNotFound;
            }
        }

        public void CacheTargetMembers()
        {
            var members = new List<string>();
            foreach (CsvFieldImportMap map in _Param.FieldImportMaps)
            {
                if (map.CacheObject)
                    members.Add(map.TargetName);
            }
            _ImportEngine.CacheXpObjectTypes(_Param.ObjectTypeInfo, members, _Param.Session);
        }

        public static void CreateTemplate(ImportCsvFileParamBase paramObj, ITypeInfo objTypeInfo)
        {
            if (paramObj.ObjectTypeInfo != objTypeInfo)
                paramObj.ObjectTypeInfo = objTypeInfo;

            var templateMemberNames = new List<string>();

            foreach (var m in objTypeInfo.Members)
            {
                if (m.IsVisible && m.IsPersistent && !m.IsReadOnly)
                {
                    templateMemberNames.Add(m.Name);
                }
            }

            var stream = string.Join(",", templateMemberNames).ToStream();

            if (paramObj.TemplateFile == null)
                paramObj.TemplateFile = new FileData(paramObj.Session);
            paramObj.TemplateFile.LoadFromStream("Template.csv", stream);
            paramObj.Session.CommitTransaction();
        }

        public CsvImportOptions Options { get; set; }

        public class CsvImportOptions
        {
            private ImportCsvFileLogic _Logic;

            public CsvImportOptions(ImportCsvFileLogic logic)
            {
                _Logic = logic;
            }

            public bool CreateMembers
            {
                get
                {
                    return _Logic._ImportEngine.Options.CreateMembers;
                }
                set
                {
                    _Logic._ImportEngine.Options.CreateMembers = value;
                }
            }

            public bool CacheObjects
            {
                get
                {
                    return _Logic._ImportEngine.Options.CacheObjects;
                }
                set
                {
                    _Logic._ImportEngine.Options.CacheObjects = value;
                }
            }
        }

    }
}
