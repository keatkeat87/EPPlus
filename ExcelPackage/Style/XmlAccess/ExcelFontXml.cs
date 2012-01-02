﻿/*******************************************************************************
 * You may amend and distribute as you like, but don't remove this header!
 *
 * EPPlus provides server-side generation of Excel 2007/2010 spreadsheets.
 * See http://www.codeplex.com/EPPlus for details.
 *
 * Copyright (C) 2011  Jan Källman
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See the GNU Lesser General Public License for more details.
 *
 * The GNU Lesser General Public License can be viewed at http://www.opensource.org/licenses/lgpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 *
 * All code and executables are provided "as is" with no warranty either express or implied. 
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 * Code change notes:
 * 
 * Author							Change						Date
 * ******************************************************************************
 * Jan Källman		                Initial Release		        2009-10-01
 * Jan Källman		License changed GPL-->LGPL 2011-12-16
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace OfficeOpenXml.Style.XmlAccess
{
    /// <summary>
    /// Xml access class for fonts
    /// </summary>
    public sealed class ExcelFontXml : StyleXmlHelper
    {
        internal ExcelFontXml(XmlNamespaceManager nameSpaceManager)
            : base(nameSpaceManager)
        {
            _name = "";
            _size = 0;
            _family = int.MinValue;
            _scheme = "";
            _color = _color = new ExcelColorXml(NameSpaceManager);
            _bold = false;
            _italic = false;
            _strike = false;
            _underline = false; ;
            _verticalAlign = "";
        }
        internal ExcelFontXml(XmlNamespaceManager nsm, XmlNode topNode) :
            base(nsm, topNode)
        {
            _name = GetXmlNodeString(namePath);
            _size = (float)GetXmlNodeDecimal(sizePath);
            _family = GetXmlNodeInt(familyPath);
            _scheme = GetXmlNodeString(schemePath);
            _color = new ExcelColorXml(nsm, topNode.SelectSingleNode(_colorPath, nsm));
            _bold = (topNode.SelectSingleNode(boldPath, NameSpaceManager) != null);
            _italic = (topNode.SelectSingleNode(italicPath, NameSpaceManager) != null);
            _strike = (topNode.SelectSingleNode(strikePath, NameSpaceManager) != null);
            _underline = (topNode.SelectSingleNode(underLinedPath, NameSpaceManager) != null);
            _verticalAlign = GetXmlNodeString(verticalAlignPath);
        }
        internal override string Id
        {
            get
            {
                return Name + "|" + Size + "|" + Family + "|" + Color.Id + "|" + Scheme + "|" + Bold.ToString() + "|" + Italic.ToString() + "|" + Strike.ToString() + "|" + VerticalAlign + "|" + UnderLine.ToString();
            }
        }
        const string namePath = "d:name/@val";
        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Scheme = "";        //Reset schema to avoid corrupt file if unsupported font is selected.
                _name = value;
            }
        }
        const string sizePath = "d:sz/@val";
        float _size;
        public float Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        const string familyPath = "d:family/@val";
        int _family;
        public int Family
        {
            get
            {
                return _family;
            }
            set
            {
                _family=value;
            }
        }
        ExcelColorXml _color = null;
        const string _colorPath = "d:color";
        public ExcelColorXml Color
        {
            get
            {
                return _color;
            }
            internal set 
            {
                _color=value;
            }
        }
        const string schemePath = "d:scheme/@val";
        string _scheme="";
        public string Scheme
        {
            get
            {
                return _scheme;
            }
            private set
            {
                _scheme=value;
            }
        }
        const string boldPath = "d:b";
        bool _bold;
        public bool Bold
        {
            get
            {
                return _bold;
            }
            set
            {
                _bold=value;
            }
        }
        const string italicPath = "d:i";
        bool _italic;
        public bool Italic
        {
            get
            {
                return _italic;
            }
            set
            {
                _italic=value;
            }
        }
        const string strikePath = "d:strike";
        bool _strike;
        public bool Strike
        {
            get
            {
                return _strike;
            }
            set
            {
                _strike=value;
            }
        }
        const string underLinedPath = "d:u";
        bool _underline;
        public bool UnderLine
        {
            get
            {
                return _underline;
            }
            set
            {
                _underline=value;
            }
        }
        const string verticalAlignPath = "d:vertAlign/@val";
        string _verticalAlign;
        public string VerticalAlign
        {
            get
            {
                return _verticalAlign;
            }
            set
            {
                _verticalAlign=value;
            }
        }
        public void SetFromFont(System.Drawing.Font Font)
        {
            Name=Font.Name;
            //Family=fnt.FontFamily.;
            Size=(int)Font.Size;
            Strike=Font.Strikeout;
            Bold = Font.Bold;
            UnderLine=Font.Underline;
            Italic=Font.Italic;
        }
        internal ExcelFontXml Copy()
        {
            ExcelFontXml newFont = new ExcelFontXml(NameSpaceManager);
            newFont.Name = Name;
            newFont.Size = Size;
            newFont.Family = Family;
            newFont.Scheme = Scheme;
            newFont.Bold = Bold;
            newFont.Italic = Italic;
            newFont.UnderLine = UnderLine;
            newFont.Strike = Strike;
            newFont.VerticalAlign = VerticalAlign;
            newFont.Color = Color.Copy();
            return newFont;
        }

        internal override XmlNode CreateXmlNode(XmlNode topElement)
        {
            TopNode = topElement;

            if (_bold) CreateNode(boldPath); else DeleteAllNode(boldPath);
            if (_italic) CreateNode(italicPath); else DeleteAllNode(italicPath);
            if (_strike) CreateNode(strikePath); else DeleteAllNode(strikePath);
            if (_underline) CreateNode(underLinedPath); else DeleteAllNode(underLinedPath);
            if (_verticalAlign!="") SetXmlNodeString(verticalAlignPath, _verticalAlign.ToString());
            SetXmlNodeString(sizePath, _size.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (_color.Exists)
            {
                CreateNode(_colorPath);
                TopNode.AppendChild(_color.CreateXmlNode(TopNode.SelectSingleNode(_colorPath, NameSpaceManager)));
            }
            SetXmlNodeString(namePath, _name);
            if(_family>int.MinValue) SetXmlNodeString(familyPath, _family.ToString());
            if (_scheme != "") SetXmlNodeString(schemePath, _scheme.ToString());

            return TopNode;
        }
    }
}
