param
(
    $httpVerb
)

try
{
	if (!$env:SCRIPTSERVICES_VERSION)
	{
		Write-Host "Running natively"
	}

    Write-Output ("HTTP '{0}' request" -f $httpVerb)
}
catch
{
	
}