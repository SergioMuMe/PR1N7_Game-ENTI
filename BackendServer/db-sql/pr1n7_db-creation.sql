/* 
!!!!!!!!!!!!!!!!!!!!!!

      PROYECTO PR1N7
      
      SERGIO MURILLO
      MARC CAYMEL
      ROGER BUJAN

      1º DAM-VIOD ENTI 

!!!!!!!!!!!!!!!!!!!!!!!
*/

/*index
############################################# 
#                                           # 
#          CREACION DE LA DATABASE          #
#                                           # 
#  Se debe respetar el orden de creación    # 
#  ya que en el propio CREATE TABLE         #
#  se especifican las FOREIGN KEYS          #
#                                           # 
############################################# 
*/

DROP DATABASE `pr1n7`;
CREATE DATABASE /*!32312 IF NOT EXISTS*/ `pr1n7` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `pr1n7`;

DROP TABLE IF EXISTS `levels`;
CREATE TABLE `levels` (
  `id_level` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `timeDev` FLOAT UNSIGNED NOT NULL,
  PRIMARY KEY (`id_level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

LOCK TABLES `levels` WRITE;
INSERT INTO `levels` 
VALUES 
(1,8),
(2,10),
(3,12),
(4,20),
(5,25);
UNLOCK TABLES;

DROP TABLE IF EXISTS `profiles`;
CREATE TABLE `profiles` (
  `id_profile` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL,
  `username` varchar(15) NOT NULL,
  `password` varchar(32) NOT NULL,
  PRIMARY KEY (`id_profile`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

LOCK TABLES `profiles` WRITE;
INSERT INTO `profiles` VALUES (1,'pr1n7_DEV','enti','c46ffbd10c7515f2ea618632c11e44a5');
UNLOCK TABLES;

DROP TABLE IF EXISTS `scores`;
CREATE TABLE `scores` (
  `id_score` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_profile` INT(10) UNSIGNED NOT NULL,
  `id_level` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id_score`),
  KEY `id_profile` (`id_profile`),
  KEY `id_level` (`id_level`),
  `finished` TINYINT(1) NOT NULL,
  `timeBeated` TINYINT(1) NOT NULL,
  `batteryCollected` TINYINT(1) NOT NULL,
  `playerRecord` FLOAT UNSIGNED NOT NULL,
  `allAtOnce` TINYINT(1) NOT NULL,
  `timestamp` TIMESTAMP NOT NULL,
  CONSTRAINT `scores_ibfk_1` FOREIGN KEY (`id_profile`) REFERENCES `profiles` (`id_profile`),
  CONSTRAINT `scores_ibfk_2` FOREIGN KEY (`id_level`) REFERENCES `levels` (`id_level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `progress`;
CREATE TABLE `progress` (
  `id_progress` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_profile` INT(10) UNSIGNED NOT NULL,
  `id_level` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id_progress`),
  KEY `id_profile` (`id_profile`),
  KEY `id_level` (`id_level`),
  `levelUnblockedFLAG` TINYINT(1) NOT NULL,
  `firstTimeFLAG` TINYINT(1) NOT NULL,
  `finished` TINYINT(1) NOT NULL,
  `timeBeated` TINYINT(1) NOT NULL,
  `batteryCollected` TINYINT(1) NOT NULL,
  `playerRecord` FLOAT UNSIGNED NOT NULL,
  `allAtOnce` TINYINT(1) NOT NULL,
  CONSTRAINT `progress_ibfk_1` FOREIGN KEY (`id_profile`) REFERENCES `profiles` (`id_profile`),
  CONSTRAINT `progress_ibfk_2` FOREIGN KEY (`id_level`) REFERENCES `levels` (`id_level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `game_versions`;
CREATE TABLE `game_versions` (
  `id_gameversion` INT(10) UNSIGNED PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `version` VARCHAR(32) NOT NULL,
  `numberOfLevels` INT(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

LOCK TABLES `game_versions` WRITE;
INSERT INTO `game_versions` VALUES (1,'alpha',5);
UNLOCK TABLES;

/*index
############################## 
#                            # 
#          FUNCTIONS         #
#              &             #
#          PROCEDURES        #
#                            # 
##############################
*/

/*
::: VERIFICAR CREDENCIALES DE USUARIO ::: 
*/
DROP FUNCTION IF EXISTS verifyCredentials;
DELIMITER $$
CREATE FUNCTION verifyCredentials(_user VARCHAR(15), _pass VARCHAR(32))
RETURNS INT (10) UNSIGNED
BEGIN

DECLARE returnNum INT (10) UNSIGNED;

    SELECT COUNT(*) INTO @numResults
    FROM profiles
    WHERE username=_user AND `password`=MD5(_pass);

    IF (@numResults = 1) THEN
        /*OK*/
        SET returnNum = 1;        
    ELSE
        /*ERROR LOGIN*/
        SET returnNum = 0;
    END IF;

RETURN returnNum;
END $$
DELIMITER ;

/* 
::: CREAR USUARIO ::: 
*/
DROP FUNCTION IF EXISTS addUser;
DELIMITER $$
CREATE FUNCTION addUser (_name VARCHAR(15), _user VARCHAR(15), _pass VARCHAR(32)) RETURNS INT (10) UNSIGNED
BEGIN
    
    DECLARE returnState INT(10) UNSIGNED; 

    SELECT COUNT(name) INTO @totalNames
    FROM profiles
    WHERE name=_name;

    SELECT COUNT(username) INTO @totalUsernames
    FROM profiles
    WHERE username=_user;

    /*El nombre ya existe*/
    IF @totalNames != 0 THEN
        SET returnState = 1;
        RETURN returnState;
    END IF;

    /*El username ya existe*/
    IF @totalUsernames != 0 THEN
        SET returnState = 2;
        RETURN returnState;
    END IF;

    /*Username y name disponibles*/
    INSERT INTO `profiles` (`name`, `username`, `password`) VALUES (_name,_user,_pass);
    SET returnState = 0;
    
RETURN returnState;

END $$
DELIMITER ;

/* 
::: AÑADIR SCORE ::: 
*/
DROP FUNCTION IF EXISTS addScore;
DELIMITER $$
CREATE FUNCTION addScore (
    _idProfile INT(10) UNSIGNED,
    _idLevel INT(10) UNSIGNED,
    _finished TINYINT(1),
    _timeBeated TINYINT(1),
    _batteryCollected TINYINT(1),
    _allAtOnce TINYINT(1),
    _playerRecord FLOAT UNSIGNED,
    _timestamp TIMESTAMP
    ) 
    RETURNS INT (10) UNSIGNED
BEGIN
    
    DECLARE returnState INT(10) UNSIGNED; 

    INSERT INTO `scores` (
        `id_profile`, 
        `id_level`,
        `finished`,
        `timeBeated`,
        `batteryCollected`,
        `playerRecord`,
        `allAtOnce`,
        `timestamp`
        ) 
    VALUES (
        _idProfile,
        _idLevel,
        _finished,
        _timeBeated,
        _batteryCollected,
        _allAtOnce,
        _playerRecord,
        _timestamp
        );
    
    SET returnState = 0;
    
RETURN returnState;

END $$
DELIMITER ;

/* 
::: CONSEGUIR HIGH SCORE ::: 
*/

DROP PROCEDURE IF EXISTS getHighScore;
DELIMITER $$
CREATE PROCEDURE getHighScore (
    _idLevel INT(10) UNSIGNED,
    _limit INT(10) UNSIGNED
    ) 
BEGIN
    
    SELECT *
    FROM `scores`
    WHERE id_level = _idLevel
    ORDER BY playerRecord ASC
    LIMIT _limit;

END $$
DELIMITER ;

/* 
::: SET SETMAXLEVELS VARIABLE INTERNA ::: 
*/
DROP FUNCTION IF EXISTS setMaxLevels;
DELIMITER $$
CREATE FUNCTION setMaxLevels()
RETURNS INT (10) UNSIGNED
BEGIN

DECLARE returnNum INT (10) UNSIGNED;

    SELECT numberOfLevels INTO @maxLevels
    FROM game_versions
    ORDER BY id_gameversion DESC 
    LIMIT 1;

    SET returnNum = @maxLevels;

RETURN returnNum;
END $$
DELIMITER ;

/* 
::: CREA TODO EL PROGRESO DE UN JUGADOR, INICIALIZA A 0 ::: 
*/
DROP FUNCTION IF EXISTS createProgress;
DELIMITER $$
CREATE FUNCTION createProgress (_idProfile INT(10) UNSIGNED)
RETURNS INT (10) UNSIGNED
BEGIN

    DECLARE returnState INT(10) UNSIGNED; 
    DECLARE i INT(10) UNSIGNED DEFAULT 2; 
    DECLARE maxLevels INT (10) UNSIGNED;
    SET maxLevels = (SELECT setMaxLevels());

    /*LVL 0 - DEV ROOM*/
    INSERT INTO `progress` (
        `id_profile`, 
        `id_level`, `levelUnblockedFLAG`, `firstTimeFLAG`,
        `finished`, `timeBeated`, `batteryCollected`,
        `playerRecord`, `allAtOnce`
        ) 
    VALUES (
        _idProfile,
        0, 0, 1,
        0, 0, 0,
        999, 0
        );
    
    /*LVL 1 - FIRST LEVEL*/
    INSERT INTO `progress` (
        `id_profile`, 
        `id_level`, `levelUnblockedFLAG`, `firstTimeFLAG`,
        `finished`, `timeBeated`, `batteryCollected`,
        `playerRecord`, `allAtOnce`
        ) 
    VALUES (
        _idProfile,
        1, 1, 1,
        0, 0, 0,
        999, 0
        );

    /*LVL I - OTHER LEVELS*/
    WHILE i <= maxLevels DO 
        INSERT INTO `progress` (
            `id_profile`, 
            `id_level`, `levelUnblockedFLAG`, `firstTimeFLAG`,
            `finished`, `timeBeated`, `batteryCollected`,
            `playerRecord`, `allAtOnce`
            ) 
        VALUES (
            _idProfile,
            i, 0, 1,
            0, 0, 0,
            999, 0
            );
        SET i=i+1;
    END WHILE;
    
    SET returnState = 0;
    
RETURN returnState;
END $$
DELIMITER ;
/*Creamos progreso de la cuenta DEV*/
SELECT createProgress(1);

/* 
::: ACTUALIZA EL PROGRESO DEL JUGADOR ::: 
*/
DROP FUNCTION IF EXISTS updateProgress;
DELIMITER $$
CREATE FUNCTION updateProgress (
    _idProfile INT(10) UNSIGNED,
    _idLevel INT(10) UNSIGNED,
    _finished TINYINT(1),
    _timeBeated TINYINT(1),
    _batteryCollected TINYINT(1),
    _playerRecord FLOAT UNSIGNED,
    _allAtOnce TINYINT(1)
    ) 
RETURNS INT (10) UNSIGNED
BEGIN
    
    DECLARE returnState INT(10) UNSIGNED; 
    DECLARE maxLevels INT (10) UNSIGNED;
    SET maxLevels = (SELECT setMaxLevels());

    /*UPDATE LEVEL JUGADO*/
    UPDATE `progress`
    SET
        `levelUnblockedFLAG` = true,
        `firstTimeFLAG` = false,
        `finished` = _finished,
        `timeBeated` = _timeBeated,
        `batteryCollected` = _batteryCollected,
        `playerRecord` = _playerRecord,
        `allAtOnce` = _allAtOnce
    WHERE 
        id_profile = _idProfile 
        AND
        id_level = _idLevel;

    /*UPDATE NEXT LEVEL JUGADO*/
    IF _idLevel = maxLevels THEN
        SET returnState = 0;
        RETURN returnState;    
    END IF;

    UPDATE `progress`
    SET
        `levelUnblockedFLAG` = true
    WHERE 
        id_profile = _idProfile 
        AND
        id_level = _idLevel+1;

    SET returnState = 0;
    
RETURN returnState;

END $$
DELIMITER ;