// Developer Express Code Central Example:
// How to generate and assign a sequential number for a business object within a database transaction, while being a part of a successful saving process (XAF)
// 
// This version of the http://www.devexpress.com/scid=E2620 example is primarily
// intended for XAF applications, because all the required operations to generate
// sequences are managed within the base persistent class. However, this code can
// be used in a regular non-XAF application based on XPO, as well.
// 
// 
// For more
// convenience, this solution is organized as a reusable module -
// GenerateUserFriendlyId.Module, which you may want to copy into your project 'as
// is' and then add it as required via the Module or Application designers
// (http://documentation.devexpress.com/#Xaf/CustomDocument2828). This module
// consists of several key parts:
// 
// 1. Sequence, SequenceGenerator, and
// SequenceGeneratorInitializer - auxiliary classes that take the main part in
// generating user-friendly identifiers.
// Take special note that the last two
// classes need to be initialized in the ModuleUpdater and ModuleBase descendants
// of your real project.
// 
// 2. UserFriendlyIdPersistentObject - a base persistent
// class that subscribes to XPO's Session events and delegates calls to the core
// classes above.
// 
// 3. IUserFriendlyIdDomainComponent - a base domain component
// that should be implemented by all domain components that require the described
// functionality. Take special note that such derived components must still use the
// BasePersistentObject as a base class during the registration, e.g.:
// XafTypesInfo.Instance.RegisterEntity("Document", typeof(IDocument),
// typeof(BasePersistentObject)); IMPORTANT NOTES
// 1. Address, Contact, and
// IDocument are business objects that demonstrate the use of the described
// functionality for XPO and DC respectively.
// 
// 2. The sequential number
// functionality shown in this example does not work with shared parts
// (http://documentation.devexpress.com/#Xaf/DevExpressExpressAppDCITypesInfo_RegisterSharedParttopic)
// (a part of the Domain Components (DC) technology) in the current version,
// because it requires a custom base class, which is not allowed for shared
// parts.
// 
// 3. This solution is not yet tested in the middle-tier and
// SecuredObjectSpaceProvider scenario and most likely, it will have to be modified
// to support its specifics.
// 
// 4. As an alternative, you can use a more simple
// solution that is using the DistributedIdGeneratorHelper.Generate method as shown
// in the FeatureCenter demo ("%Public%\Documents\DXperience 12.2 Demos\eXpressApp
// Framework\FeatureCenter\CS\FeatureCenter.Module\KeyProperty\GuidKeyPropertyObject.cs"
// ).
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E2829

using System;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using GenerateUserFriendlyId.Module.Utils;

namespace GenerateUserFriendlyId.Module.BusinessObjects {
    //Dennis: Uncomment this code if you want to have the SequentialNumber column created in each derived class table.
    [NonPersistent]
    public abstract class UserFriendlyIdPersistentObject : BaseObject, ISupportSequentialNumber {
        private long _SequentialNumber;
        private static SequenceGenerator sequenceGenerator;
        private static object syncRoot = new object(); 
        public UserFriendlyIdPersistentObject(Session session)
            : base(session) {
        }
        [Browsable(false)]
        //Dennis: Comment out this code if you do not want to have the SequentialNumber column created in each derived class table.
        [Indexed(Unique = false)]
        public long SequentialNumber {
            get { return _SequentialNumber; }
            set { SetPropertyValue("SequentialNumber", ref _SequentialNumber, value); }
        }
        private void OnSequenceGenerated(long newId) {
            SequentialNumber = newId;
        }
        protected override void OnSaving() {
            try {
                base.OnSaving();
                if (Session.IsNewObject(this) && !typeof(NestedUnitOfWork).IsInstanceOfType(Session))
                    GenerateSequence();
            } catch {
                CancelSequence();
                throw;
            }
        }
        public void GenerateSequence() {
            lock (syncRoot) {
                Dictionary<string, bool> typeToExistsMap = new Dictionary<string, bool>();
                foreach (object item in Session.GetObjectsToSave())
                    typeToExistsMap[Session.GetClassInfo(item).FullName] = true;
                if (sequenceGenerator == null)
                    sequenceGenerator = new SequenceGenerator(typeToExistsMap);
                SubscribeToEvents();
                OnSequenceGenerated(sequenceGenerator.GetNextSequence(ClassInfo));
            }
        }
        private void AcceptSequence() {
            lock (syncRoot) {
                if (sequenceGenerator != null) {
                    try {
                        sequenceGenerator.Accept();
                    } finally {
                        CancelSequence();
                    }
                }
            }
        }
        private void CancelSequence() {
            lock (syncRoot) {
                UnSubscribeFromEvents();
                if (sequenceGenerator != null) {
                    sequenceGenerator.Close();
                    sequenceGenerator = null;
                }
            }
        }
        private void Session_AfterCommitTransaction(object sender, SessionManipulationEventArgs e) {
            AcceptSequence();
        }
        private void Session_AfterRollBack(object sender, SessionManipulationEventArgs e) {
            CancelSequence();
        }
        private void Session_FailedCommitTransaction(object sender, SessionOperationFailEventArgs e) {
            CancelSequence();
        }
        private void SubscribeToEvents() {
            if (!(Session is NestedUnitOfWork)) {
                Session.AfterCommitTransaction += Session_AfterCommitTransaction;
                Session.AfterRollbackTransaction += Session_AfterRollBack;
                Session.FailedCommitTransaction += Session_FailedCommitTransaction;
            }
        }
        private void UnSubscribeFromEvents() {
            if (!(Session is NestedUnitOfWork)) {
                Session.AfterCommitTransaction -= Session_AfterCommitTransaction;
                Session.AfterRollbackTransaction -= Session_AfterRollBack;
                Session.FailedCommitTransaction -= Session_FailedCommitTransaction;
            }
        }
    }
}