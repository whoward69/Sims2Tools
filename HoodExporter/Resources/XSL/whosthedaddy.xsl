<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sims2tools="http://picknmixmods.com/Sims2Tools/SaxonExtns">

  <xsl:output name="csv" method="text" omit-xml-declaration="yes" indent="no"/>

  <!-- Output strings -->
  <xsl:variable name="space"><xsl:text> </xsl:text></xsl:variable>
  <xsl:variable name="quote"><xsl:text>"</xsl:text></xsl:variable>
  <xsl:variable name="csvQuote"><xsl:text>""</xsl:text></xsl:variable>
  <xsl:variable name="separator"><xsl:text>,</xsl:text></xsl:variable>
  <xsl:variable name="newLine"><xsl:text>
</xsl:text></xsl:variable>

  <!-- Ignore anything not explicitly matched or selected -->
  <xsl:template match="/|@*|node()|comment()" priority="0">
    <xsl:apply-templates select="@*|node()"/>
  </xsl:template>

  <!-- Create the WhosTheDaddy.csv file -->
  <xsl:template match="main" priority="1">
    <xsl:variable name="hoodCode" select="./idno/@ownerName"/>
    <xsl:variable name="hoodName" select="./ctss/language[1]/item[@index='0x0000']/text"/>

    <xsl:result-document format="csv" href="WhosTheDaddy.csv">
      <xsl:text>"Sim ID","Family Name","Given Name","Hours Pregnant","Babies","Father ID","Father Family Name","Father Given Name"</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./ngbh/sims/tokens/standard/item[@guid='0xED4CB1A4']" mode="whosthedaddy"/>
    </xsl:result-document>
  </xsl:template>

  <xsl:template match="item" mode="whosthedaddy">
    <xsl:variable name="simid" select="../../@simId"/>
    <xsl:variable name="fatherid" select="sims2tools:asHex(./data/@i1)"/>

    <xsl:value-of select="$simid"/>

    <xsl:apply-templates select="/hood/main/sdsc[@simId=$simid]" mode="whosthedaddy"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./data/@i2"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./data/@i3"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$fatherid"/>

    <xsl:apply-templates select="/hood/main/sdsc[@simId=$fatherid]" mode="whosthedaddy"/>

    <xsl:value-of select="$newLine"/>
  </xsl:template>

  <!-- Output a Sim's name -->
  <xsl:template match="sdsc" mode="whosthedaddy">
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="familyName"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="givenName"/>
    <xsl:value-of select="$quote"/>
  </xsl:template>
</xsl:stylesheet>
