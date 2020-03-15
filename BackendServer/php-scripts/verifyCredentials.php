<?php

/* http://192.168.1.162/verifyCredentials.php?username=%22enti%22&password=%22enti%22 */
/* Hay que pasar los parametros con comillas al ser strings, sino SQL no lo reconoce */

/*VARIABLES*/
$_user=$_GET["username"];
$_pass=$_GET["password"];


$LINK=mysqli_connect("localhost", "enti", "enti", "pr1n7");

if (!$LINK)
{
	echo "Error 001: LINK FAIL";
}

$QUERY = "SELECT verifyCredentials($_user,$_pass) AS response;";
$RESULT = $LINK->query($QUERY);

if(!$RESULT)
{
	echo "Error 002: RESULT NULL";
}

$LIST=[];
while($ROW = $RESULT->fetch_assoc()){
    array_push($LIST, $ROW);
}

$DATA=json_encode($LIST);
$DATA = trim($DATA,'[]');
mysqli_close($LINK);

echo($DATA);

?>