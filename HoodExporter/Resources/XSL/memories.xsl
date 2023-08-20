<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sims2tools="http://picknmixmods.com/Sims2Tools/SaxonExtns">

  <xsl:output name="csv" method="text" omit-xml-declaration="yes" indent="no"/>

  <!-- Output strings -->
  <xsl:variable name="space"><xsl:text> </xsl:text></xsl:variable>
  <xsl:variable name="quote"><xsl:text>"</xsl:text></xsl:variable>
  <xsl:variable name="separator"><xsl:text>,</xsl:text></xsl:variable>
  <xsl:variable name="newLine"><xsl:text>
</xsl:text></xsl:variable>

  <!-- Ignore anything not explicitly matched or selected -->
  <xsl:template match="/|@*|node()|comment()" priority="0">
    <xsl:apply-templates select="@*|node()"/>
  </xsl:template>

  <!-- Create the Memories.csv file -->
  <xsl:template match="main" priority="1">
    <xsl:variable name="hoodCode" select="./idno/@ownerName"/>
    <xsl:variable name="hoodName" select="./ctss/language[1]/item[@index='0x0000']/text"/>

    <xsl:result-document format="csv" href="Memories.csv">
      <xsl:text>"Memory GUID","Memory Name","Memory Title","Owner ID","Owner Name","About GUID","About Name"</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./ngbh/sims/tokens/standard/item[@flags='0x0003']" mode="memories">
      </xsl:apply-templates>
    </xsl:result-document>
  </xsl:template>

  <!-- Find all the memories -->
  <xsl:template match="item" mode="memories">
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="@guid"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="sims2tools:asObjectName(@guid)"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="sims2tools:asObjectTitle(@guid)"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="sims2tools:asHex(./data/@i4)"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:call-template name="ownerName">
      <xsl:with-param name="ownerId" select="sims2tools:asHex(./data/@i4)"/>
    </xsl:call-template>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="concat(sims2tools:asHex(./data/@i6, 4),sims2tools:asHexNoPrefix(./data/@i5, 4))"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:call-template name="aboutName">
      <xsl:with-param name="aboutGuid" select="concat(sims2tools:asHex(./data/@i6, 4),sims2tools:asHexNoPrefix(./data/@i5, 4))"/>
    </xsl:call-template>

  <xsl:value-of select="$newLine"/>
  </xsl:template>
  
  <xsl:template name="ownerName">
    <xsl:param name="ownerId"/>
    <xsl:variable name="owner" select="/hood/main/sdsc[@simId=$ownerId]"/>
    
    <xsl:value-of select="$quote"/>
    <xsl:choose>
      <xsl:when test="$owner">
        <xsl:value-of select="$owner/givenName"/>
        <xsl:value-of select="$space"/>
        <xsl:value-of select="$owner/familyName"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$ownerId"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:value-of select="$quote"/>
  </xsl:template>

  <xsl:template name="aboutName">
    <xsl:param name="aboutGuid"/>
    <xsl:variable name="owner" select="/hood/main/sdsc[@simGuid=$aboutGuid]"/>
    
    <xsl:value-of select="$quote"/>
    <xsl:choose>
      <xsl:when test="$aboutGuid = '0x6DD33865'">
        <xsl:value-of select="'Mystery Sim'"/>
      </xsl:when>
      <xsl:when test="$owner">
        <xsl:value-of select="$owner/givenName"/>
        <xsl:value-of select="$space"/>
        <xsl:value-of select="$owner/familyName"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$aboutGuid"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:value-of select="$quote"/>
  </xsl:template>
</xsl:stylesheet>
