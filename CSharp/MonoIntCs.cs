LoadAssembly("System.Xml.Linq");

using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Xml.Linq;

var uri = new Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250");
XNamespace wb = XNamespace.Get("http://www.worldbank.org");
var rawdata = new WebClient().DownloadString(uri);
var sr = new StringReader(rawdata.Substring(3)); // Some encoding issue with UTF8 BOM and mono XML parser
var doc = XDocument.Load(sr);

Func<XElement, bool> HasValue = (d) =>
{
	var v = d.Element(wb + "value");
	return v != null && !v.IsEmpty;
};

Func<XElement, int> GetValue = (d) => int.Parse(d.Element(wb + "value").Value);

Func<XElement, string> GetCountry = d => d.Element(wb + "country").Value;

var data = doc.Root.DescendantsAndSelf(wb + "data").Where(HasValue);
var pops = data.Select(v => Tuple.Create(GetValue(v), GetCountry(v)));
var pops2 = from v in data
			select Tuple.Create(GetValue(v), GetCountry(v));
var res = pops.OrderByDescending(x => x).Take(5);

// Show output:
res.ToList()
