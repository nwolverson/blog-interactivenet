#r "System.Xml"
#r "System.Xml.Linq"
#r "System.Core"

using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Xml.Linq;

XNamespace wb = XNamespace.Get("http://www.worldbank.org");

bool HasValue(XElement d)
{
	var v = d.Element(wb + "value");
	return v != null && !v.IsEmpty;
}

int GetValue(XElement d)
{
	return int.Parse(d.Element(wb + "value").Value);
}

string GetCountry(XElement d)
{
	return d.Element(wb + "country").Value;
}

var uri = new Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250");
var rawdata = new WebClient().DownloadString(uri);
var sr = new StringReader(rawdata);
var doc = XDocument.Load(sr);

var data = doc.Root.DescendantsAndSelf(wb + "data").Where(HasValue);
var pops = data.Select(v => Tuple.Create(GetValue(v), GetCountry(v)));
var pops2 = from v in data
			select Tuple.Create(GetValue(v), GetCountry(v));
var res = pops.OrderByDescending(x => x).Take(5);

// Show output:
res.ToList()
