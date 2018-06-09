#region Copyright

// Copyright 2011 - Charles Nurse

#endregion

// ReSharper disable InconsistentNaming

using System;

namespace FamilyTreeProject.DomainServices.Tests.Common
{
    public static class TestConstants
    {
        public const int ID_Exists = 1;
        public const string ID_FatherId = "1";
        public const string ID_MotherId = "2";
        public const string ID_WifeId = "3";
        public const string ID_HusbandId = "4";
        public const int ID_NotFound = 999;

        public const string IND_FirstName = "Foo{0}";
        public const string IND_LastName = "Bar";
        public const int IND_LastNameCount = 10;
        public const string IND_AltLastName = "Car";
        public const string IND_UpdateFirstName = "John";
        public const string IND_UpdateLastName = "Smith";

        public const string CIT_Page = "Page{0}";
        public const string CIT_Text = "Text{0}";

        public const string EVN_Date = "Date{0}";
        public const string EVN_Place = "Place{0}";

        public const string REP_Name = "Name{0}";
        public const string REP_Address = "Address{0}";

        public const string SRC_Author = "Author{0}";
        public const string SRC_Title = "Title{0}";

        public const int PAGE_NotFound = 42;
        public const int PAGE_RecordCount = 5;
        public const int PAGE_TotalCount = 22;

        public static readonly string TREE_Id = Guid.Empty.ToString();
    }
}