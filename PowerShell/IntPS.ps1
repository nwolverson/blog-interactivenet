Add-Type -AssemblyName  System.Xml.Linq

$wb = [System.Xml.Linq.XNamespace]::Get("http://www.worldbank.org")

$uri = New-Object uri "http://api.worldbank.org/countries/indicators/EN.URB.LCTY?date=2009:2009&per_page=250"
$rawdata = (New-Object System.Net.WebClient).DownloadString($uri);
$sr = New-Object System.IO.StringReader($rawdata);
$doc = [System.Xml.Linq.XDocument]::Load($sr);

function hasValue ([System.Xml.Linq.XElement] $d) {
    $v = $d.Element($wb + "value");
    $v -ne $null -and -not $v.IsEmpty;
}
function getValue ([System.Xml.Linq.XElement] $d) {
    [int] $d.Element($wb + "value").Value;
}
function getCountry ([System.Xml.Linq.XElement] $d) {
    $d.Element($wb + "country").Value;
}

$data = $doc.Root.DescendantsAndSelf($wb + "data") | 
    Where-Object { hasValue $_ };

# Do it with a list rather than unstructured objects: shorter but output is less nice
#$pops = $data | ForEach-Object { , ((getValue $_), (getCountry $_)) };
#$sorted = $pops | Sort-Object -Descending { [int] $_[0] }
#$sorted | Select-Object -First 5 | Write-Output

$pops = $data | ForEach-Object { New-Object -TypeName PSObject `
    -Property @{Country=getCountry $_;Population=getValue $_} }
# Alternative object construction: 
#$pops = $data | Select-Object -Property @{Name="Country";Expression={getCountry $_}},@{Name="Population";Expression={(getValue $_)}} 

$sorted = $pops | Sort-Object -Property Population -Descending
$sorted | Select-Object -First 5 | Write-Output

# Shorter form: 
#$data | % { New-Object -ty psobject -prop @{Country=getCountry $_;Population=getValue $_} } | sort -p Population -des | select -f 5 

