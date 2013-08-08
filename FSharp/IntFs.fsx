#r "System.Core"
#r "System.Xml.Linq"

open System
open System.Net
open System.IO
open System.Linq
open System.Xml.Linq

let uri = System.Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250")
let rawdata = (new WebClient()).DownloadString(uri)
let sr = new StringReader(rawdata)
let doc = XDocument.Load(sr)
let wb = XNamespace.Get("http://www.worldbank.org")

let hasValue (d: XElement) =
    match d.Element(wb + "value") with 
    | null -> false
    | v -> not v.IsEmpty 

let getValue (d: XElement) =
    d.Element(wb + "value").Value |> int
let getCountry (d: XElement) =
    d.Element(wb + "country").Value

let data = doc.Root.DescendantsAndSelf(wb + "data") |> Seq.where hasValue
let pops = [ for v in data -> getValue(v), getCountry(v) ]
let sorted = pops |> List.sort |> List.rev |> Seq.take 5 

