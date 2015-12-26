using Xafology.Utils;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public delegate void ImportCsvFileEventHandler();

    public abstract class ImportCsvFileLogic
    {
        public event ImportCsvFileEventHandler BeforeImport;
        public event ImportCsvFileEventHandler AfterImport;

        public ImportErrorInfo ErrorInfo { get; set; }
        protected readonly Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase paramBase;
        protected readonly XpoImportEngine importEngine;
        protected CsvReader csvReader;
        private readonly XafApplication application;

        protected XafApplication Application
        {
            get
            {
                return application;
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

        public ImportCsvFileLogic(XafApplication application, Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase param)
        {
            paramBase = param;
            ErrorInfo = null;
            CancellationTokenSource = null;
            importEngine = new XpoImportEngine(application);
            Options = new CsvImportOptions(this);
            _objTypeInfo = param.ObjectTypeInfo;
            application = application;
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
                return importEngine.XpObjectsNotFound;
            }
        }

        public void CacheTargetMembers()
        {
            var members = new List<string>();
            foreach (Xafology.ExpressApp.Xpo.Import.Parameters.CsvFieldImportMap map in paramBase.FieldImportMaps)
            {
                if (map.CacheObject)
                    members.Add(map.TargetName);
            }
            importEngine.CacheXpObjectTypes(paramBase.ObjectTypeInfo, members, paramBase.Session);
        }

        public static void CreateTemplate(Xafology.ExpressApp.Xpo.Import.Parameters.ImportCsvFileParamBase paramObj, ITypeInfo objTypeInfo)
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
            private Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileLogic _Logic;

            public CsvImportOptions(Xafology.ExpressApp.Xpo.Import.Logic.ImportCsvFileLogic logic)
            {
                _Logic = logic;
            }

            public bool CreateMembers
            {
                get
                {
                    return _Logic.importEngine.Options.CreateMembers;
                }
                set
                {
                    _Logic.importEngine.Options.CreateMembers = value;
                }
            }

            public bool CacheObjects
            {
                get
                {
                    return _Logic.importEngine.Options.CacheObjects;
                }
                set
                {
                    _Logic.importEngine.Options.CacheObjects = value;
                }
            }
        }

    }
}
