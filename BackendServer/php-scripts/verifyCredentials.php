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
/* TODO: REVISAR LO DE LOS [ ] */
$DATA = trim($DATA,'[]');
mysqli_close($LINK);

echo($DATA);

/*
CONTROL DE SEGURIDAD
Cuando funcione todo, 
- poner HTTPS-certificado
- securizar código (lo miraremos en acceso de datos)
- securizar MV

Tabla de IDs, para el HASH
Hago SELECT Random de un num no asignado.
ID_number (1000 al 9999), ID_usuario (default 0)
Tabla de control.


scores: arreglar camelcase


Dreamhost: low price, pero puta mierda
Bluehost:



*/

?>

