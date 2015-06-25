param([string]$v = "", [string]$m = "", [string]$b = "release")

$latestTag = Invoke-Expression 'git describe --abbrev=0 --tags 2>$null'
if($latestTag -eq $null) {
	$latestTag = "v0.0.0"
}

function CreateTag($tag) {
	git tag -a $tag -m $m
	git checkout $b
	git rebase master
	git push origin $b
	git checkout master
}

function TriggerBuild() {
	$url="http://ci.smv.br/job/LM_Core/build?token=D10C42FA01A2435EA3BF92D8D2C5C4FB"
	(New-Object System.Net.WebClient).DownloadString("$url");
}

if($latestTag -match 'v(?<major>\d+)\.?(?<minor>\d+)\.(?<patch>\d)+') {

	$majorCurrent = [convert]::ToInt32($matches['major'])
	$minorCurrent = [convert]::ToInt32($matches['minor'])
	$patchCurrent = [convert]::ToInt32($matches['patch'])

	if($v -eq "major") {
		$majorNumber = $majorCurrent + 1
		CreateTag "v$majorNumber.0.0"
	} elseif ($v -eq "minor") {
		$minorNumber = $minorCurrent + 1
		CreateTag "v$majorCurrent.$minorNumber.0"
	} elseif ($v -eq "patch") {
		$patchNumber = $patchCurrent + 1
		CreateTag "v$majorCurrent.$minorCurrent.$patchNumber"
	} else {
		Write-Host "incorrect bump."
	}	
} else {
	Write-Host "$latestTag is a invalid tag."
}
