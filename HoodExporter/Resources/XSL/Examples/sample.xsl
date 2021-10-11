<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sims2tools="http://picknmixmods.com/Sims2Tools/SaxonExtns">

  <!-- Shallow copy recursively every node and attribute -->
  <xsl:template match="/|@*|node()|comment()" priority="0">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
	</xsl:copy>
  </xsl:template>
  
  <!--
  Custom functions for data conversion, see the following templates for examples
    asInt(hex) - convert a hex number to an integer, eg asInt('0x0000000A') returns 10
	  asHex(int) - convert an int into a 8 digit hex number with an 0x prefix, eg asHex(10) returns 0x0000000A
	  asHex(int, digits)- convert an int into an N digit hex number with an 0x prefix, eg asHex(10, 2) returns 0x0A
	  asHexNoPrefix(int) - convert an int into a 8 digit hex number, eg asHex(10) returns 0000000A
	  asHexNoPrefix(int, digits)- convert an int into an N digit hex number, eg asHex(10, 2) 0A
	  asBinary(int) - convert an int into a 8 digit binary number, eg asBinary(10) returns 00001010
	  asBinary(int, digits)- convert an int into an N digit hex number with an 0x prefix, eg asHex(10, 4) returns 1010
	  asYesNo(string) - if string is null, empty, "No", "N" or will parse as zero (eg, 0, 0x00, etc) returns No, otherwise returns Yes
	  asYesNo(string, 1) - returns only the first character (N or Y) from asYesNo(string)
  -->

  <!-- Convert any simId attribute to an int -->
  <xsl:template match="@simId" priority="1">
    <xsl:attribute name="simId">
      <xsl:value-of select="sims2tools:asInt(.)"/>
    </xsl:attribute>
  </xsl:template>

  <!-- Convert npcType to 0x00 format -->
  <xsl:template match="sdsc/base/@npcType" priority="1">
    <xsl:attribute name="npcType">
      <xsl:value-of select="sims2tools:asHex(., 2)"/>
    </xsl:attribute>
  </xsl:template>

  <!-- Convert ghostFlags to binary format -->
  <xsl:template match="sdsc/base/@ghostFlags" priority="1">
    <xsl:attribute name="ghostFlags">
      <xsl:value-of select="sims2tools:asBinary(.)"/>
    </xsl:attribute>
  </xsl:template>

  <!-- Convert unlinked="0/1" to unlinked="N/Y" -->
  <xsl:template match="sdsc/@unlinked" priority="1">
    <xsl:attribute name="unlinked">
      <xsl:value-of select="sims2tools:asYesNo(., 1)"/>
    </xsl:attribute>
  </xsl:template>
</xsl:stylesheet>
