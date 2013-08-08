#r """..\packages\FSharp.Data.1.1.5\lib\net40\FSharp.Data.dll"""
open FSharp.Data

let data = WorldBankData.GetDataContext()

// This is easy
data.Countries.``United Kingdom``.Indicators.``Population growth (annual %)`` |> Seq.toList

// To get data for all countries is not so easy - API fetches pages per region or country.
