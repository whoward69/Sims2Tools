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

  <!-- Create the Careers.csv file -->
  <xsl:template match="main" priority="1">
    <xsl:variable name="hoodCode" select="./idno/@ownerName"/>
    <xsl:variable name="hoodName" select="./ctss/language[1]/item[@index='0x0000']/text"/>

    <xsl:result-document format="csv" href="Careers.csv">
      <xsl:text>"Sim GUID","Sim ID","Family Name","Given Name","Age","School","Grade","Job","Level","Performance"</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="/hood/main/sdsc[string-length(normalize-space(familyName)) gt 0]" mode="careers"/>
    </xsl:result-document>
  </xsl:template>

  <!-- Find all the careers data -->
  <xsl:template match="sdsc" mode="careers">
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="@simGuid"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="@simId"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="familyName"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="givenName"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/@lifestage"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:choose>
      <xsl:when test="starts-with(base/career/@school, '0x')">
        <xsl:value-of select="sims2tools:asObjectTitle(base/career/@school)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="base/career/@school"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/career/@schoolGrade"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:choose>
      <xsl:when test="starts-with(base/career/@job, '0x')">
        <xsl:value-of select="sims2tools:asObjectTitle(base/career/@job)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="base/career/@job"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/career/@jobLevel"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/career/@jobPerformance"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$newLine"/>
  </xsl:template>
</xsl:stylesheet>
