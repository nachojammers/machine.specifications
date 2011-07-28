﻿using System;
using System.Linq;
using System.Xml;

using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.UnitTestFramework;

using Machine.Specifications.Utility.Internal;

namespace Machine.Specifications.ReSharperRunner.Presentation
{
  public abstract class FieldElement : Element, ISerializableElement
  {
    readonly string _fieldName;

    protected FieldElement(MSpecUnitTestProvider provider,
                           Element parent,
                           ProjectModelElementEnvoy projectEnvoy,
                           string declaringTypeName,
                           string fieldName,
                           bool isIgnored)
      : base(provider, parent, projectEnvoy, declaringTypeName, isIgnored || parent.Explicit)
    {
      _fieldName = fieldName;
      State = UnitTestElementState.Valid;
    }

    public override string Id
    {
      get { return string.Format("{0}.{1}", TypeName, FieldName); }
    }

    public override string ShortName
    {
      get { return FieldName; }
    }

    public string FieldName
    {
      get { return _fieldName; }
    }

    public override string GetPresentation()
    {
      return String.Format("{0}{1}{2}",
                           GetTitlePrefix(),
                           String.IsNullOrEmpty(GetTitlePrefix()) ? String.Empty : " ",
                           FieldName.ToFormat());
    }

    public override IDeclaredElement GetDeclaredElement()
    {
      ITypeElement declaredType = GetDeclaredType();
      if (declaredType == null)
      {
        return null;
      }

      return declaredType.EnumerateMembers(FieldName, false)
        .Where(member => member as IField != null)
        .FirstOrDefault();
    }

    public override bool Equals(object obj)
    {
      if (!base.Equals(obj))
      {
        return false;
      }

      var other = obj as FieldElement;
      if (other == null)
      {
        return false;
      }

      return Equals(Parent, other.Parent) && FieldName == other.FieldName;
    }

    public override int GetHashCode()
    {
      int result = base.GetHashCode();
      result = 29 * result + TypeName.GetHashCode();
      result = 29 * result + FieldName.GetHashCode();
      return result;
    }

    public virtual void WriteToXml(XmlElement parent)
    {
      parent.SetAttribute("projectId", GetProject().GetPersistentID());
      parent.SetAttribute("typeName", TypeName);
      parent.SetAttribute("methodName", FieldName);
      parent.SetAttribute("isIgnored", Explicit.ToString());
    }
  }
}