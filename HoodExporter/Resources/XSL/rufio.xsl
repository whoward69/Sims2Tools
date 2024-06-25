<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:sims2tools="http://picknmixmods.com/Sims2Tools/SaxonExtns">

  <xsl:output name="csv" method="text" omit-xml-declaration="yes" indent="no"/>

  <!-- Output strings -->
  <xsl:variable name="quote"><xsl:text>"</xsl:text></xsl:variable>
  <xsl:variable name="csvQuote"><xsl:text>""</xsl:text></xsl:variable>
  <xsl:variable name="separator"><xsl:text>,</xsl:text></xsl:variable>
  <xsl:variable name="newLine"><xsl:text>
</xsl:text></xsl:variable>

  <!-- Ignore anything not explicitly matched or selected -->
  <xsl:template match="/|@*|node()|comment()" priority="0">
    <xsl:apply-templates select="@*|node()"/>
  </xsl:template>

  <!-- Create the ExportedLots.csv and ExportedSims.csv files -->
  <xsl:template match="main" priority="1">
    <xsl:variable name="hoodCode" select="./idno/@ownerName"/>
    <xsl:variable name="hoodName" select="./ctss/language[1]/item[@index='0x0000']/text"/>

    <xsl:result-document format="csv" href="ExportedLots.csv">
      <xsl:text>hood,HoodName,LotInstance,HouseNumber,HouseName</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./ltxt" mode="exportLots">
        <xsl:with-param name="hoodCode" select="$hoodCode"/>
        <xsl:with-param name="hoodName" select="$hoodName"/>
      </xsl:apply-templates>
    </xsl:result-document>

    <xsl:result-document format="csv" href="ExportedSims.csv">
      <xsl:text>hood,HoodName,NID,FirstName,LastName,SimDescription,FamilyInstance,HouseholdName,HouseNumber,AvailableCharacterData,Unlinked,ParentA,ParentB,Spouse,BodyType,NPCType,SchoolType,Grade,CareerPerformance,Career,CareerLevel,ZodiacSign,Aspiration,Gender,LifeSection,AgeDaysLeft,PrevAgeDays,AgeDuration,BlizLifelinePoints,LifelinePoints,LifelineScore,GenActive,GenNeat,GenNice,GenOutgoing,GenPlayful,Active,Neat,Nice,Outgoing,Playful,Animals,Crime,Culture,Entertainment,Environment,Fashion,FemalePreference,Food,Health,MalePreference,Money,Paranormal,Politics,School,Scifi,Sports,Toys,Travel,Weather,Work,Body,Charisma,Cleaning,Cooking,Creativity,Fatness,Logic,Mechanical,Romance,IsAtUniversity,UniEffort,UniGrade,UniTime,UniSemester,UniInfluence,UniMajor,Species,Salary,PrimaryAspiration,SecondaryAspiration,HobbyPredestined,LifetimeWant</xsl:text>
      <xsl:value-of select="$newLine"/>

      <xsl:apply-templates select="./sdsc" mode="exportSims">
        <xsl:with-param name="hoodCode" select="$hoodCode"/>
        <xsl:with-param name="hoodName" select="$hoodName"/>
      </xsl:apply-templates>
    </xsl:result-document>
  </xsl:template>

  <!-- Per lot data for ExportedLots.csv -->
  <xsl:template match="ltxt" mode="exportLots">
    <xsl:param name="hoodCode"/>
    <xsl:param name="hoodName"/>

    <xsl:value-of select="$hoodCode"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace($hoodName, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(./@lotId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(./@lotId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace(./name, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$newLine"/>
  </xsl:template>

  <!-- Per sim data for ExportedSims.csv -->
  <xsl:template match="sdsc" mode="exportSims">
    <xsl:param name="hoodCode"/>
    <xsl:param name="hoodName"/>

    <xsl:variable name="simId" select="./@simId"/>
    <xsl:variable name="familyId" select="./@familyId"/>

    <xsl:value-of select="$hoodCode"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace($hoodName, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt($simId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace(./givenName, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace(./familyName, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace(./description, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt($familyId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="concat($quote, replace(//str[@instanceId=$familyId]/language[1]/item[@index='0x0000']/text, $quote, $csvQuote), $quote)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(//fami[@familyId=$familyId]/@lotId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asYesNo(./@characterFile, 1)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asYesNo(./@unlinked, 1)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(//srel[@simId=$simId and @relFamily='Child'][1]/@towardsId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(//srel[@simId=$simId and @relFamily='Child'][2]/@towardsId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asInt(//srel[@simId=$simId and @relFamily='Spouses'][1]/@towardsId)"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@bodyFlags"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@npcType"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/career/@school"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/career/@schoolGrade"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/career/@jobPerformance"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/career/@job"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/career/@jobLevel"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@zodiac"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationPrimary"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@gender"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@lifestage"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@age"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@agePrevDays"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/@ageDuration"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationBlizPoints"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationPoints"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationScore"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterGenetic/@active"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterGenetic/@neat"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterGenetic/@nice"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterGenetic/@outgoing"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterGenetic/@playful"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterCurrent/@active"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterCurrent/@neat"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterCurrent/@nice"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterCurrent/@outgoing"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/characterCurrent/@playful"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@animals"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@crime"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@culture"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@entertainment"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@environment"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@fashion"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@prefFemale"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@food"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@health"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@prefMale"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@money"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@paranormal"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@politics"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@school"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@scifi"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@sports"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@toys"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@travel"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@weather"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/interests/@work"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@body"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@charisma"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@cleaning"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@cooking"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@creativity"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@fatness"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@logic"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@mechanical"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/skills/@romance"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="sims2tools:asYesNo(./uni/@onCampus, 1)"/>
    <xsl:value-of select="$separator"/>
    <xsl:if test="./uni/@onCampus = 1">
      <xsl:value-of select="./uni/@effort"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="./uni/@grade"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="./uni/@time"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="./uni/@semester"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="./uni/@influence"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="./uni/@major"/>
      <xsl:value-of select="$separator"/>
    </xsl:if>
    <xsl:if test="./uni/@onCampus != 1">
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$separator"/>
      <xsl:value-of select="$separator"/>
    </xsl:if>
    <xsl:value-of select="./nl/@species"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./ofb/@salary"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationPrimary"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./base/aspiration/@aspirationSecondary"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./ft/hobbies/@predestined"/>
    <xsl:value-of select="$separator"/>
    <xsl:value-of select="./ft/lta/@aspiration"/>
    <xsl:value-of select="$newLine"/>
  </xsl:template>
</xsl:stylesheet>
