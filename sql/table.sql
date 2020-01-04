CREATE TABLE `hna`.`records`(
    `dslno` INT NOT NULL AUTO_INCREMENT,
    `ddate` VARCHAR(500) NOT NULL,
    `dtime` VARCHAR(500) NOT NULL,
    `dmessage` VARCHAR(500) NOT NULL,
    `dpath` VARCHAR(500) NOT NULL,
    `dstatus` VARCHAR(500) NOT NULL,
    PRIMARY KEY(`dslno`)
) ENGINE = INNODB;