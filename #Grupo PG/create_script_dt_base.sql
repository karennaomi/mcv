-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema grupopg
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema grupopg
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `grupopg` DEFAULT CHARACTER SET latin1 ;
USE `grupopg` ;

-- -----------------------------------------------------
-- Table `grupopg`.`tb_banco`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_banco` (
  `IdBanco` INT(11) NULL DEFAULT NULL,
  `NomeBanco` VARCHAR(50) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_conta_contabil`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_conta_contabil` (
  `IdContaContabil` INT(11) NULL DEFAULT NULL,
  `NomeConta` VARCHAR(50) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_endereco`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_endereco` (
  `IdEndereco` INT(11) NULL DEFAULT NULL,
  `Logradouro` VARCHAR(250) NULL DEFAULT NULL,
  `IdCidade` INT(11) NULL DEFAULT NULL,
  `UF` VARCHAR(50) NULL DEFAULT NULL,
  `Numero` INT(11) NULL DEFAULT NULL,
  `Complemento` VARCHAR(50) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_equipe`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_equipe` (
  `IdEquipe` INT(11) NULL DEFAULT NULL,
  `IdFuncionario` INT(11) NULL DEFAULT NULL,
  `DtCriacao` DATETIME NULL DEFAULT NULL,
  `NomeEquipe` VARCHAR(50) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_faturamento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_faturamento` (
  `IdFatura` INT(11) NOT NULL,
  `IdOS` INT(11) NOT NULL,
  `DtFatura` DATETIME NULL DEFAULT NULL,
  `DtVencimento` DATETIME NULL DEFAULT NULL,
  `ValorFatura` DATETIME NULL DEFAULT NULL,
  `IdContaContabil` INT(11) NULL DEFAULT NULL,
  `FlFaturado` BIT(1) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_os`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_os` (
  `IdOS` INT(11) NOT NULL AUTO_INCREMENT,
  `IdBanco` INT(11) NULL DEFAULT NULL,
  `TxSinalizacao` VARCHAR(250) NULL DEFAULT NULL,
  `TxLocalizacaoMaquina` VARCHAR(50) NULL DEFAULT NULL,
  `NomeContato` VARCHAR(50) NULL DEFAULT NULL,
  `TelefoneContato` VARCHAR(50) NULL DEFAULT NULL,
  `IdContaContabil` INT(11) NULL DEFAULT NULL,
  `IdTipoMaquina` INT(11) NULL DEFAULT NULL,
  `IdTipoInstalacao` INT(11) NULL DEFAULT NULL,
  `IdTipoFixacao` INT(11) NULL DEFAULT NULL,
  `IdEquipe` INT(11) NULL DEFAULT NULL,
  `IdPC` INT(11) NULL DEFAULT NULL,
  `TxDefeito` VARCHAR(150) NULL DEFAULT NULL,
  `FlHabilitada` BIT(1) NULL DEFAULT NULL,
  `PaUnificado` VARCHAR(50) NULL DEFAULT NULL,
  `DtEntrega` DATETIME NULL DEFAULT NULL,
  `HrEntrega` DATETIME NULL DEFAULT NULL,
  `DtFinalizacao` DATETIME NULL DEFAULT NULL,
  `DtChegada` DATETIME NULL DEFAULT NULL,
  `HrChegada` DATETIME NULL DEFAULT NULL,
  `DtChegadaTransportadora` DATETIME NULL DEFAULT NULL,
  `HrChegadaTransportadora` DATETIME NULL DEFAULT NULL,
  `VlrOrcamento` DECIMAL(9,2) NULL DEFAULT NULL,
  `DtInclusaoRegistro` DATETIME NULL DEFAULT NULL,
  `TxSerie` VARCHAR(50) NULL DEFAULT NULL,
  `NrOrdemServico` VARCHAR(50) NULL DEFAULT NULL,
  `FlAtivo` BIT(1) NULL DEFAULT NULL,
  PRIMARY KEY (`IdOS`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_tipo_fixacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_tipo_fixacao` (
  `IdTipoFixacao` INT(11) NULL DEFAULT NULL,
  `NomeTipoFixacao` CHAR(10) CHARACTER SET 'utf8' NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_tipo_instalacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_tipo_instalacao` (
  `IdTipoInstalacao` INT(11) NULL DEFAULT NULL,
  `NomeTipoInstalacao` VARCHAR(50) NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_tipo_maquina`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_tipo_maquina` (
  `IdTipoMaquina` INT(11) NULL DEFAULT NULL,
  `NomeTipoMaquina` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `grupopg`.`tb_usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `grupopg`.`tb_usuario` (
  `IdUsuario` INT(11) NOT NULL AUTO_INCREMENT,
  `NmUsuario` VARCHAR(150) NOT NULL,
  `Email` VARCHAR(150) NULL DEFAULT NULL,
  `IdTipoUsuario` INT(11) NOT NULL,
  `DtCriacao` DATETIME NOT NULL,
  `DtAlteracao` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`IdUsuario`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
