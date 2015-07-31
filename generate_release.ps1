param([string]$v = "", [string]$m = "", [string]$b = "release")

$ciHost="http://ci.smv.br"
$devJob="LM_Core"
$homJob="LM_Core"
$devToken="D10C42FA01A2435EA3BF92D8D2C5C4FB"
$homToken="D10C42FA01A2435EA3BF92D8D2C5C4FB"

$latestTag = Invoke-Expression 'git describe --abbrev=0 --tags 2>$null'
if($latestTag -eq $null) {
	$latestTag = "v0.0.0"
}

function CreateVersionFile($tag) {
	New-Item $projFolder\version.txt -type file -force -value $tag
	git add $projFolder\version.txt
	git commit -am "new version $tag"
}

function PushTag($tag) {
	Write-Host ("Creating tag {0}" -f $tag)
	git tag -a $tag -m $m
	git push origin $tag
}

function PushToReleaseBranch() {
	git checkout $b
	git rebase master
	git push origin $b
	git checkout master
}

function TriggerBuild() {
	$url="$ciHost/job/$devJob/build?token=$devToken"
	Write-Host ("Triggering build on {0}" -f $url) 
	(New-Object System.Net.WebClient).DownloadString("$url");
}

function TriggerBuildWithTag($tag) {
	$url="$ciHost/job/$homJob/buildWithParameters?token=$homToken&tag=$tag"
	Write-Host ("Triggering build on {0}" -f $url) 
	(New-Object System.Net.WebClient).DownloadString("$url");
}

function Publish($tag) {
	CreateVersionFile $tag
	PushTag $tag
	TriggerBuildWithTag $tag
}

if($latestTag -match 'v(?<major>\d+)\.?(?<minor>\d+)\.(?<patch>\d)+') {

	$majorCurrent = [convert]::ToInt32($matches['major'])
	$minorCurrent = [convert]::ToInt32($matches['minor'])
	$patchCurrent = [convert]::ToInt32($matches['patch'])

	if($v -eq "") {
		PushToReleaseBranch
		TriggerBuild
	} elseif($v -eq "major") {
		$majorNumber = $majorCurrent + 1
		Publish "v$majorNumber.0.0"
	} elseif ($v -eq "minor") {
		$minorNumber = $minorCurrent + 1
		Publish "v$majorCurrent.$minorNumber.0"
	} elseif ($v -eq "patch") {
		$patchNumber = $patchCurrent + 1
		Publish "v$majorCurrent.$minorCurrent.$patchNumber"
	} else {
		Write-Host "Invalid version: $v"
	}	
} else {
	Write-Host "$latestTag is a invalid tag."
}
