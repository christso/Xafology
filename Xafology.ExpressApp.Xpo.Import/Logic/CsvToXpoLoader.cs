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

    public abstract class CsvToXpoLoader : IFieldMapper
    {

        #region Fields

        public event ImportCsvFileEventHandler BeforeImport;
        public event ImportCsvFileEventHandler AfterImport;

        public ImportErrorInfo ErrorInfo { get; set; }
        protected readonly Xafology.ExpressApp.Xpo.Import.Parameters.ImportParamBase paramBase;
        protected readonly XpoMapper xpoMapper;
        protected CsvReader csvReader;
        private readonly XafApplication application;

        private CsvToXpoInserter inserter;
        private CsvToXpoUpdater updater;

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

        #endregion

        #region Constructor

        public CsvToXpoLoader(XafApplication application, Xafology.ExpressApp.Xpo.Import.Parameters.ImportParamBase param)
        {
            paramBase = param;
            ErrorInfo = null;
            CancellationTokenSource = null;
            xpoMapper = new XpoMapper(application);
            Options = xpoMapper.Options;
            _objTypeInfo = param.ObjectTypeInfo;
            this.application = application;
        }

        #endregion

        #region Methods


        public abstract void Execute();

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
                return xpoMapper.XpObjectsNotFound;
            }
        }

        public void CacheTargetMembers()
        {
            var members = new List<string>();
            foreach (Xafology.ExpressApp.Xpo.Import.Parameters.FieldMap map in paramBase.FieldImportMaps)
            {
                if (map.CacheObject)
                    members.Add(map.TargetName);
            }
            xpoMapper.CacheXpObjectTypes(paramBase.ObjectTypeInfo, members, paramBase.Session);
        }

        public static void CreateTemplate(Xafology.ExpressApp.Xpo.Import.Parameters.ImportParamBase paramObj, ITypeInfo objTypeInfo)
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

        #endregion

        #region Properties

        public IImportOptions Options { get; set; }

        protected XafApplication Application
        {
            get
            {
                return application;
            }
        }

        public XpoMapper XpoMapper
        {
            get
            {
                return xpoMapper;
            }
        }
        #endregion

    }
}
