#r """..\packages\FSharp.Data.1.1.5\lib\net40\FSharp.Data.dll"""
#r "System.Core"
#r "System.Xml.Linq"

type XmlProvider = FSharp.Data.XmlProvider<"EN.URB.LCTY.xml">

open System
open System.Net

let uri = System.Uri("http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250")
let rawdata = (new WebClient()).DownloadString(uri)
let xmlData = XmlProvider.Parse(rawdata);

// Unfortunate pluralisation
let data = xmlData.GetDatas() |> Seq.where(fun v -> v.Value.IsSome)

let pops = [ for v in data -> v.Value.Value, v.Country.Value ]
let sorted = pops |> List.sort |> List.rev
sorted |> Seq.take 5