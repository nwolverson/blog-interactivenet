import clr

clr.AddReference("System.Core");
clr.AddReference("System.Xml.Linq");

import System
from System.Net import *
from System.IO import *
from System.Linq import *
from System.Xml.Linq import *

uri = System.Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250");
data = WebClient().DownloadString(uri);
sr = StringReader(data);
doc = XDocument.Load(sr);
wb = XNamespace.Get("http://www.worldbank.org");

def hasValue(d):
    val = d.Element(wb+"value")
    return val != None and not val.IsEmpty
def getValue(d):
    return int(d.Element(wb+"value").Value)
def getCountry(d):
    return d.Element(wb+"country").Value

pops = [(getValue(n), getCountry(n)) 
        for n in doc.Root.DescendantsAndSelf(wb+"data") 
        if hasValue(n)]
pops.sort(reverse=True)
pops[:5]