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
  `id_score` INT(10) UNSIGNED PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `finished` TINYINT(1) NOT NULL,
  `timeBeated` TINYINT(1) NOT NULL,
  `batteryCollected` TINYINT(1) NOT NULL,
  `playerRecord` FLOAT UNSIGNED NOT NULL,
  `allAtOnce` TINYINT(1) NOT NULL,
  `timestamp` INT(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

DROP TABLE IF EXISTS `gameversions`;
CREATE TABLE `gameversions` (
  `id_gameversion` INT(10) UNSIGNED PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `version` VARCHAR(32) NOT NULL,
  `numberOfLevels` INT(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

LOCK TABLES `gameversions` WRITE;
INSERT INTO `gameversions` VALUES (1,'alpha',5);
UNLOCK TABLES;

DROP TABLE IF EXISTS `scores_history`;
CREATE TABLE `scores_history` (
  `id_score_history` INT(10) UNSIGNED PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `id_score` INT(10) UNSIGNED NOT NULL,
  `id_profile` INT(10) UNSIGNED NOT NULL,
  `id_level` INT(10) UNSIGNED NOT NULL,
  `level_timeDev` FLOAT UNSIGNED NOT NULL,
  `id_gameversion` INT(10) UNSIGNED NOT NULL,
  KEY `id_score` (`id_score`),
  KEY `id_profile` (`id_profile`),
  KEY `id_level` (`id_level`),
  KEY `id_gameversion` (`id_gameversion`),
  CONSTRAINT `scores_history_ibfk_1` FOREIGN KEY (`id_score`) REFERENCES `scores` (`id_score`),
  CONSTRAINT `scores_history_ibfk_2` FOREIGN KEY (`id_profile`) REFERENCES `profiles` (`id_profile`),
  CONSTRAINT `scores_history_ibfk_3` FOREIGN KEY (`id_level`) REFERENCES `levels` (`id_level`),
  CONSTRAINT `scores_history_ibfk_4` FOREIGN KEY (`id_gameversion`) REFERENCES `gameversions` (`id_gameversion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


DROP TABLE IF EXISTS `progress`;
CREATE TABLE `progress` (
  `id_progress` INT(10) UNSIGNED PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `id_profile` INT(10) UNSIGNED NOT NULL,
  `id_level` INT(10) UNSIGNED NOT NULL,
  `levelUnblockedFLAG` TINYINT(1) NOT NULL,
  `firstTimeFLAG` TINYINT(1) NOT NULL,
  `id_score` INT(10) UNSIGNED NOT NULL,
  `id_gameversion` INT(10) UNSIGNED NOT NULL,
  KEY `id_profile` (`id_profile`),
  KEY `id_level` (`id_level`),
  KEY `id_score` (`id_score`),
  KEY `id_gameversion` (`id_gameversion`),
  CONSTRAINT `progress_ibfk_1` FOREIGN KEY (`id_score`) REFERENCES `scores` (`id_score`),
  CONSTRAINT `progress_ibfk_2` FOREIGN KEY (`id_profile`) REFERENCES `profiles` (`id_profile`),
  CONSTRAINT `progress_ibfk_3` FOREIGN KEY (`id_level`) REFERENCES `levels` (`id_level`),
  CONSTRAINT `progress_ibfk_4` FOREIGN KEY (`id_gameversion`) REFERENCES `gameversions` (`id_gameversion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
    _idGameversion INT(10) UNSIGNED,
    _idProfile INT(10) UNSIGNED,
    _idLevel INT(10) UNSIGNED,
    _finished TINYINT(1),
    _timeBeated TINYINT(1),
    _batteryCollected TINYINT(1),
    _playerRecord FLOAT UNSIGNED,
    _allAtOnce TINYINT(1),
    _timestamp INT(11),
    addHistory TINYINT(1)
    ) 
    RETURNS INT (10) UNSIGNED
BEGIN
    
    DECLARE returnState INT(10) UNSIGNED; 
    DECLARE scoreID INT(10) UNSIGNED; 
    DECLARE actualTimeDev INT(10) UNSIGNED; 

    INSERT INTO `scores` (
        `finished`,
        `timeBeated`,
        `batteryCollected`,
        `playerRecord`,
        `allAtOnce`,
        `timestamp`
        ) 
    VALUES (
        _finished,
        _timeBeated,
        _batteryCollected,
        _playerRecord,
        _allAtOnce,
        _timestamp
        );

    /*Guardamos ID Score insertado*/
    SET scoreID = (
        SELECT last_insert_id()
        );

    IF addHistory=false THEN
        RETURN scoreID;
    END IF;

    /*Guardamos el tiempo record actual definido por DEV*/
    SELECT timeDev INTO actualTimeDev
    FROM levels
    WHERE id_level = _idLevel
    LIMIT 1;
    
    
    /*Guardamos registro de la partida para metricas internas*/
    INSERT INTO `scores_history` (
        `id_score`,
        `id_profile`,
        `id_level`,
        `level_timeDev`,
        `id_gameversion`
        )
    VALUES (
        scoreID,
        _idProfile,
        _idLevel,
        actualTimeDev,
        _idGameversion
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
    _limit INT(10) UNSIGNED,
    _idGameversion INT(10) UNSIGNED
    ) 
BEGIN
    
    SELECT 
        scores.playerRecord AS playerRecord, 
        scores.finished AS finished, 
        scores.timeBeated AS timeBeated, 
        scores.batteryCollected AS batteryCollected, 
        scores.allAtOnce AS allAtOnce 
    FROM scores_history
    LEFT JOIN scores
        ON scores_history.id_score = scores.id_score
    LEFT JOIN levels
        ON scores_history.id_level = levels.id_level
    LEFT JOIN gameversions
        ON scores_history.id_gameversion = gameversions.id_gameversion
    WHERE 
        scores_history.id_level = _idLevel 
        AND gameversions.id_gameversion = _idGameversion
    ORDER BY scores.playerRecord ASC
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
    FROM gameversions
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
CREATE FUNCTION createProgress (
    _idProfile INT(10) UNSIGNED,
    _idGameversion INT(10) UNSIGNED,
    addHistory TINYINT(1),
    unixTimestamp INT(11)
    )
RETURNS INT (10) UNSIGNED
BEGIN

    DECLARE returnState INT(10) UNSIGNED; 
    DECLARE i INT(10) UNSIGNED DEFAULT 2; 
    DECLARE scoreID INT(10) UNSIGNED; 
    DECLARE maxLevels INT (10) UNSIGNED;
    
    SET maxLevels = (SELECT setMaxLevels());

    /*LVL 1 - FIRST LEVEL*/
    SET scoreID = (
        SELECT addScore(
            _idGameversion,
            _idProfile,
            1, 0, 0, 0, 999, 0, unixTimestamp, addHistory
            )
    );

    INSERT INTO `progress` (
        `id_profile`, 
        `id_level`, `levelUnblockedFLAG`, `firstTimeFLAG`,
        `id_score`, `id_gameversion`
        ) 
    VALUES (
        _idProfile,
        1, 1, 1,
        scoreID, _idGameversion        
    );

    /*LVL I - OTHER LEVELS*/
    WHILE i <= maxLevels DO 
        
        SET scoreID = (
            SELECT addScore(
                _idGameversion,
                _idProfile,
                i, 0, 0, 0, 999, 0, unixTimestamp, addHistory
                )
        );

        INSERT INTO `progress` (
            `id_profile`, 
            `id_level`, `levelUnblockedFLAG`, `firstTimeFLAG`,
            `id_score`, `id_gameversion`
            ) 
        VALUES (
            _idProfile,
            i, 0, 1,
            scoreID, _idGameversion
        );

        SET i=i+1;
    END WHILE;
    
    SET returnState = 0;
    
RETURN returnState;
END $$
DELIMITER ;

/*Creamos progreso de la cuenta DEV*/
SELECT unix_timestamp() INTO @time ;
SELECT createProgress(1,1,0, @time ) ;

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
    _allAtOnce TINYINT(1),
    _idGameversion INT(10) UNSIGNED,
    _unixTimestamp INT(10) UNSIGNED
    ) 
RETURNS INT (10) UNSIGNED
BEGIN
    
    DECLARE returnState INT(10) UNSIGNED; 
    DECLARE maxLevels INT (10) UNSIGNED;
    DECLARE scoreID INT(10) UNSIGNED;
    SET maxLevels = (SELECT setMaxLevels());

    SET scoreID = (
            SELECT addScore(
                _idGameversion,
                _idProfile,
                _idLevel, 
                _finished, 
                _timeBeated, 
                _batteryCollected, 
                _playerRecord, 
                _allAtOnce,
                _unixTimestamp, 1
                )
        );

    /*UPDATE LEVEL JUGADO*/
    UPDATE `progress`   
    SET
        `firstTimeFLAG` = false,
        `id_score` = scoreID,
        `id_gameversion` = _idGameversion
    WHERE 
        id_profile = _idProfile 
        AND
        id_level = _idLevel;

    /*UPDATE NEXT LEVEL*/
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