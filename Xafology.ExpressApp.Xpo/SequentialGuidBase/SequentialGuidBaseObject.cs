using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.ComponentModel;
using Xafology.Utils.SequentialGuid;

namespace Xafology.ExpressApp.Xpo.SequentialGuidBase
{
    [NonPersistent]
    public abstract class SequentialGuidBaseObject : XPCustomObject
    {
        /// <summary>
		///     <para>Set this field to <b>true</b> before profiling the application via XPO Profiler.
		/// </para>
		/// </summary>
		/// <returns> </returns>
		public static bool IsXpoProfiling = false;

        public static bool IsSequential = true;

        [Key(true), MemberDesignTimeVisibility(false), Persistent("Oid"), Browsable(false)]
        private Guid oid = Guid.Empty;

        private bool isDefaultPropertyAttributeInit;

        private XPMemberInfo defaultPropertyMemberInfo;

        private static OidInitializationMode oidInitializationMode = OidInitializationMode.OnSaving;

        /// <summary>
        ///     <para>Specifies the persistent object's identifier.
        /// </para>
        /// </summary>
        /// <value>A globally unique identifier which represents the persistent object's identifier.
        /// </value>
        [PersistentAlias("oid"), Browsable(false)]
        public Guid Oid
        {
            get
            {
                return this.oid;
            }
        }

        /// <summary>
        ///     <para>Specifies when a new GUID value is assigned to the <see cref="P:DevExpress.Persistent.BaseImpl.BaseObject.Oid" /> property. 
        /// </para>
        /// </summary>
        /// <value>An <see cref="T:DevExpress.Persistent.BaseImpl.OidInitializationMode" /> enumeration value specifying when a new GUID value is assigned to the Oid property. 
        /// </value>
        public static OidInitializationMode OidInitializationMode
        {
            get
            {
                return SequentialGuidBaseObject.oidInitializationMode;
            }
            set
            {
                SequentialGuidBaseObject.oidInitializationMode = value;
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(base.Session is NestedUnitOfWork) && base.Session.IsNewObject(this) && this.oid.Equals(Guid.Empty))
            {
                this.oid = this.NewGuid();
            }
        }

        public void GenerateOid()
        {
            this.oid = this.NewGuid();
        }

        /// <summary>
        ///     <para>Invoked when the current object is about to be initialized after its creation. 
        ///
        /// </para>
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SequentialGuidBaseObject.oidInitializationMode == OidInitializationMode.AfterConstruction)
            {
                this.oid = this.NewGuid();
            }
        }

        private Guid NewGuid()
        {
            //SequentialGuid.Create(SequentialGuidType.SequentialAsString)
            if (IsSequential)
                return SQLGuidUtil.NewSequentialId();
            else
                return XpoDefault.NewGuid();
        }

        /// <summary>
        ///     <para>Used to initialize a new instance of a BaseObject descendant, in a particular Session.
        ///
        /// </para>
        /// </summary>
        /// <param name="session">
        /// 		A <b>DevExpress.Xpo.Session</b> object which represents a persistent object's cache where the business object will be instantiated.
        ///
        ///
        /// </param>
        public SequentialGuidBaseObject(Session session) : base(session)
		{
        }

        /// <summary>
        ///     <para>Creates a new instance of the BaseObject class.
        /// </para>
        /// </summary>
        public SequentialGuidBaseObject()
        {
        }

        /// <summary>
        ///     <para>Returns a human-readable string that represents the current business object.
        /// </para>
        /// </summary>
        /// <returns>A string representing the current business object.
        /// </returns>
        public override string ToString()
        {
            if (!BaseObject.IsXpoProfiling)
            {
                if (!this.isDefaultPropertyAttributeInit)
                {
                    string text = string.Empty;
                    XafDefaultPropertyAttribute xafDefaultPropertyAttribute = XafTypesInfo.Instance.FindTypeInfo(base.GetType()).FindAttribute<XafDefaultPropertyAttribute>();
                    if (xafDefaultPropertyAttribute != null)
                    {
                        text = xafDefaultPropertyAttribute.Name;
                    }
                    else
                    {
                        DefaultPropertyAttribute defaultPropertyAttribute = XafTypesInfo.Instance.FindTypeInfo(base.GetType()).FindAttribute<DefaultPropertyAttribute>();
                        if (defaultPropertyAttribute != null)
                        {
                            text = defaultPropertyAttribute.Name;
                        }
                    }
                    if (!string.IsNullOrEmpty(text))
                    {
                        this.defaultPropertyMemberInfo = base.ClassInfo.FindMember(text);
                    }
                    this.isDefaultPropertyAttributeInit = true;
                }
                if (this.defaultPropertyMemberInfo != null)
                {
                    object value = this.defaultPropertyMemberInfo.GetValue(this);
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }
            return base.ToString();
        }
    }
}
