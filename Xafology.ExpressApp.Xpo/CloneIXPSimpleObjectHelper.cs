// Developer Express Code Central Example:
// How to clone a persistent object
// 
// This is an example for the http://www.devexpress.com/scid=A1631 Knowledge Base
// article. Please refer to the article for the explanation.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E804


using System;
using DevExpress.Xpo;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;
using System.Collections;

namespace Xafology.ExpressApp.Xpo
{
    public class CloneIXPSimpleObjectHelper
    {
        /// <summary>
        /// A dictionary containing objects from the source session as key and objects from the 
        /// target session as values
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> clonedObjects;
        Session sourceSession;
        Session targetSession;

        /// <summary>
        /// Initializes a new instance of the CloneIXPSimpleObjectHelper class.
        /// </summary>
        public CloneIXPSimpleObjectHelper(Session source, Session target)
        {
            this.clonedObjects = new Dictionary<object, object>();
            this.sourceSession = source;
            this.targetSession = target;
        }

        public T Clone<T>(T source) where T : IXPSimpleObject
        {
            return Clone<T>(source, targetSession, false);
        }
        public T Clone<T>(T source, bool synchronize) where T : IXPSimpleObject
        {
            return (T)Clone(source as IXPSimpleObject, targetSession, synchronize);
        }

        public object Clone(IXPSimpleObject source)
        {
            return Clone(source, targetSession, false);
        }
        public object Clone(IXPSimpleObject source, bool synchronize)
        {
            return Clone(source, targetSession, synchronize);
        }

        public T Clone<T>(T source, Session targetSession, bool synchronize) where T : IXPSimpleObject
        {
            return (T)Clone(source as IXPSimpleObject, targetSession, synchronize);
        }

        /// <summary>
        /// Clones and / or synchronizes the given IXPSimpleObject.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetSession"></param>
        /// <param name="synchronize">If set to true, reference properties are only cloned in case
        /// the reference object does not exist in the targetsession. Otherwise the exising object will be
        /// reused and synchronized with the source. Set this property to false when knowing at forehand 
        /// that the targetSession will not contain any of the objects of the source.</param>
        /// <returns></returns>
        public object Clone(IXPSimpleObject source, Session targetSession, bool synchronize)
        {
            if (source == null)
                return null;
            if (clonedObjects.ContainsKey(source))
                return clonedObjects[source];
            XPClassInfo targetClassInfo = targetSession.GetClassInfo(source.GetType());
            object clone = null;
            if (synchronize)
                clone = targetSession.GetObjectByKey(targetClassInfo, source.Session.GetKeyValue(source));
            if (clone == null)
                clone = targetClassInfo.CreateNewObject(targetSession);
            clonedObjects.Add(source, clone);

            foreach (XPMemberInfo m in targetClassInfo.PersistentProperties)
            {
                if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey)
                    continue;
                object val;
                if (m.ReferenceType != null)
                {
                    object createdByClone = m.GetValue(clone);
                    if ((createdByClone != null) && synchronize == false)
                        val = createdByClone;
                    else
                    {
                        val = Clone((IXPSimpleObject)m.GetValue(source), targetSession, synchronize);
                    }

                }
                else
                {
                    val = m.GetValue(source);
                }
                m.SetValue(clone, val);
            }
            foreach (XPMemberInfo m in targetClassInfo.CollectionProperties)
            {
                if (m.HasAttribute(typeof(AggregatedAttribute)))
                {
                    XPBaseCollection col = (XPBaseCollection)m.GetValue(clone);
                    XPBaseCollection colSource = (XPBaseCollection)m.GetValue(source);
                    foreach (IXPSimpleObject obj in new ArrayList(colSource))
                        col.BaseAdd(Clone(obj, targetSession, synchronize));
                }
            }
            return clone;
        }

        /// <summary>
        /// Clone object excluding collection properties.
        /// </summary>
        public static IXPSimpleObject CloneLocal(IXPSimpleObject source)
        {
            var targetSession = source.Session;
            XPClassInfo targetClassInfo = targetSession.GetClassInfo(source.GetType());

            IXPSimpleObject clone = (IXPSimpleObject)targetClassInfo.CreateNewObject(targetSession);

            foreach (var m in source.ClassInfo.Members)
            {
                if (m is DevExpress.Xpo.Metadata.Helpers.ServiceField || m.IsKey || !m.IsPublic || m.IsReadOnly) continue;
                clone.ClassInfo.GetMember(m.Name).SetValue(clone, m.GetValue(source));
            }
            return clone;
        }
    }
}
