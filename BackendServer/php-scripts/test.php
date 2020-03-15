<?php 

/*HELP RAFA, COMO HAGO QUE ME SAQUE POR CONSOLE LOG LAS COSITAS?*/
function console_log($output, $with_script_tags = true){
	$js_code = 'console.log("'.json_enconde($output, JSON_HEX_TAG).'");';


	if($with_script_tags)
	{
		$js_code = '<script>'.$js_code.'</script>';
	}
	echo $js_code;
}


$LINK=mysqli_connect("localhost", "enti", "enti", "pr1n7");



if (!$LINK)
{
	echo "Error 001: LINK FAIL";
//	exit();
}


$QUERY = "SELECT * FROM profiles";
$RESULT = $LINK->query($QUERY);

if(!$RESULT)
{
	echo "Error 002: RESULT NULL";
//	exit();
}

$LIST=[];
while($ROW = $RESULT->fetch_assoc()){
	array_push($LIST, $ROW);
}

$DATA=json_encode($LIST);

/*Necesario quitar brakets para que UNITY pueda parsear el JSON a la clase creada*/
$DATA = trim($DATA,'[]');

echo $DATA;

mysqli_close($LINK);

/*ejemplo reación objeto en php*/
$myObj->name = "John";
$myObj->age = 30;
$myObj->city = "New York";

$myJSON = json_encode($myObj);

echo $myJSON;

?>
