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

  <!-- Create the Census.txt file -->
  <xsl:template match="main" priority="1">
    <xsl:variable name="hoodCode" select="./idno/@ownerName"/>
    <xsl:variable name="hoodName" select="./ctss/language[1]/item[@index='0x0000']/text"/>

    <xsl:result-document format="csv" href="Census.txt">
      <xsl:text>"Sim GUID","Sim ID","Family Name","Given Name","Species","Gender","Life Stage","Family ID","Family Group","Lot ID","Hood","Address","Phone"</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./fami[@lotId!='0x00000000']/sims/sim" mode="census"/>
    </xsl:result-document>
  </xsl:template>

  <xsl:template match="sim" mode="census">
    <xsl:variable name="guid" select="@simGuid"/>

    <xsl:value-of select="$quote"/>
    <xsl:value-of select="$guid"/>
    <xsl:value-of select="$quote"/>

    <xsl:apply-templates select="/hood/main/sdsc[@simGuid=$guid]" mode="census"/>

    <xsl:value-of select="$newLine"/>
  </xsl:template>

  <!-- Find all the census data -->
  <xsl:template match="sdsc" mode="census">
    <xsl:variable name="familyid" select="@familyId"/>
    <xsl:variable name="lotid" select="//fami[@familyId=$familyid]/@lotId"/>

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
    <xsl:value-of select="nl/@species"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/@gender"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="base/@lifestage"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="$familyid"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//str[@instanceId=$familyid]/language[1]/item[@index='0x0000']/text"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//fami[@familyId=$familyid]/@lotId"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//ltxt[@lotId=$lotid]/../ctss/language[1]/item[@index='0x0000']/text"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//ltxt[@lotId=$lotid]/name"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//fami[@familyId=$familyid]/@hasPhone"/>
    <xsl:value-of select="$quote"/>
  </xsl:template>
</xsl:stylesheet>
