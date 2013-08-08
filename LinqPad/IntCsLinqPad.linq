<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Core.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.XML.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xml.Linq.dll</Reference>
  <Namespace>System</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Xml.Linq</Namespace>
</Query>

XNamespace wb = XNamespace.Get("http://www.worldbank.org");

Func<XElement, bool> hasValue = d => 
{
	var v = d.Element(wb + "value");
	return v != null && !v.IsEmpty;
};

Func<XElement, int> getValue = d =>
{
	return int.Parse(d.Element(wb + "value").Value);
};

Func<XElement, string> getCountry = d =>
{
	return d.Element(wb + "country").Value;
};

var uri = new Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250");
var rawdata = new WebClient().DownloadString(uri);
var sr = new StringReader(rawdata);
var doc = XDocument.Load(sr);

var data = doc.Root.DescendantsAndSelf(wb + "data").Where(hasValue);
var pops = from v in data
			select Tuple.Create(getValue(v), getCountry(v));
pops.Take(5).Dump();

var sorted = pops.OrderByDescending(x => x);
sorted.Take(5).Dump();

