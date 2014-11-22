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

using DevExpress.ExpressApp;
using GenerateUserFriendlyId.Module.BusinessObjects;
using GenerateUserFriendlyId.Module.Utils;

namespace GenerateUserFriendlyId.Module {
    public sealed partial class GenerateUserFriendlyIdModule : ModuleBase {
        public GenerateUserFriendlyIdModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            SequenceGeneratorInitializer.Register(application);
        }
    }
}