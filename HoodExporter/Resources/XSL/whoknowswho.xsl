<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sims2tools="http://picknmixmods.com/Sims2Tools/SaxonExtns">

  <xsl:output name="csv" method="text" omit-xml-declaration="yes" indent="no"/>

  <!-- Output strings -->
  <xsl:variable name="space">
    <xsl:text> </xsl:text>
  </xsl:variable>
  <xsl:variable name="quote">
    <xsl:text>"</xsl:text>
  </xsl:variable>
  <xsl:variable name="separator">
    <xsl:text>,</xsl:text>
  </xsl:variable>
  <xsl:variable name="newLine">
    <xsl:text>
</xsl:text>
  </xsl:variable>

  <!-- Ignore anything not explicitly matched or selected -->
  <xsl:template match="/|@*|node()|comment()" priority="0">
    <xsl:apply-templates select="@*|node()"/>
  </xsl:template>

  <!-- Create the WhoKnowsWho.csv file -->
  <xsl:template match="main" priority="1">
    <xsl:result-document format="csv" href="WhoKnowsWho.csv">
      <xsl:text>"From Sim GUID","From Sim ID","From Family ID","From Family Group","From Family Name","From Given Name"</xsl:text>
      <xsl:text>,"To Sim GUID","To Sim ID","To Family ID","To Family Group","To Family Name","To Given Name"</xsl:text>
      <xsl:text>,"STR","LTR","Relation","Friend","Romance","Pet Friend","Pack"</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./srel[@known='true']" mode="whoknowswho"/>
    </xsl:result-document>
  </xsl:template>

  <xsl:template match="srel" mode="whoknowswho">
    <xsl:variable name="fromId" select="@simId"/>
    <xsl:variable name="toId" select="@towardsId"/>

    <xsl:variable name="fromFamilyId" select="/hood/main/sdsc[@simId=$fromId]/@familyId"/>
    <xsl:variable name="toFamilyId" select="/hood/main/sdsc[@simId=$toId]/@familyId"/>

    <xsl:if test="$fromFamilyId!='0x00000000' and $toFamilyId!='0x00000000'">
      <xsl:apply-templates select="/hood/main/sdsc[@simId=$fromId]" mode="whoknowswho"/>

      <xsl:value-of select="$separator"/>
      <xsl:apply-templates select="/hood/main/sdsc[@simId=$toId]" mode="whoknowswho"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="@str"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="@ltr"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$quote"/>
      <xsl:choose>
        <xsl:when test="@relFamily='Spouses'">
          <xsl:text>spouse</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Parent'">
          <xsl:text>parent</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Child'">
          <xsl:text>child</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Sibling'">
          <xsl:text>sibling</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Gradparent'">
          <xsl:text>grandparent</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Grandchild'">
          <xsl:text>grandchild</xsl:text>
        </xsl:when>
        <xsl:when test="@relFamily='Aunt'">
          <xsl:variable name="fromgender" select="/hood/main/sdsc[@simId=$fromId]/base/@gender"/>
          <xsl:choose>
            <xsl:when test="$fromgender='Male'">
              <xsl:text>uncle</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>aunt</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@relFamily='Nice_Nephew'">
          <xsl:variable name="fromgender" select="/hood/main/sdsc[@simId=$fromId]/base/@gender"/>
          <xsl:choose>
            <xsl:when test="$fromgender='Male'">
              <xsl:text>nephew</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>niece</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@relFamily='Cousin'">
          <xsl:text>cousin</xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$quote"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$quote"/>
      <xsl:choose>
        <xsl:when test="@bff='true'">
          <xsl:text>bff</xsl:text>
        </xsl:when>
        <xsl:when test="@buddie='true'">
          <xsl:text>buddy</xsl:text>
        </xsl:when>
        <xsl:when test="@friend='true'">
          <xsl:text>friend</xsl:text>
        </xsl:when>
        <xsl:when test="@contact='true'">
          <xsl:text>contact</xsl:text>
        </xsl:when>
        <xsl:when test="@enemy='true'">
          <xsl:text>enemy</xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$quote"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$quote"/>
      <xsl:choose>
        <xsl:when test="@married='true'">
          <xsl:text>married</xsl:text>
        </xsl:when>
        <xsl:when test="@engaged='true'">
          <xsl:text>engaged</xsl:text>
        </xsl:when>
        <xsl:when test="@steady='true'">
          <xsl:text>steady</xsl:text>
        </xsl:when>
        <xsl:when test="@love='true'">
          <xsl:text>love</xsl:text>
        </xsl:when>
        <xsl:when test="@crush='true'">
          <xsl:text>crush</xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$quote"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$quote"/>
      <xsl:choose>
        <xsl:when test="@petBuddie='true'">
          <xsl:text>buddy</xsl:text>
        </xsl:when>
        <xsl:when test="@petFriend='true'">
          <xsl:text>friend</xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$quote"/>

      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$quote"/>
      <xsl:choose>
        <xsl:when test="@master='true'">
          <xsl:text>master</xsl:text>
        </xsl:when>
        <xsl:when test="@mine='true'">
          <xsl:text>mine</xsl:text>
        </xsl:when>
        <xsl:when test="@pack='true'">
          <xsl:text>pack</xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$quote"/>

      <xsl:value-of select="$newLine"/>
    </xsl:if>
  </xsl:template>

  <!-- Find all the who knows who data -->
  <xsl:template match="sdsc" mode="whoknowswho">
    <xsl:variable name="familyId" select="@familyId"/>

    <xsl:value-of select="$quote"/>
    <xsl:value-of select="@simGuid"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="@simId"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="$familyId"/>
    <xsl:value-of select="$quote"/>

    <xsl:value-of select="$separator"/>
    <xsl:value-of select="$quote"/>
    <xsl:value-of select="//str[@instanceId=$familyId]/language[1]/item[@index='0x0000']/text"/>
    <xsl:value-of select="$quote"/>

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
