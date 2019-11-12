using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MissionID{

	public int worldID;
	public string desc;
	public int time;
	public int hits;
	public int distance;
	public int multiplierEXP;
	public Dictionary <int,int> rewards = new Dictionary <int,int>();
	public int hitMode;
	public int objectsAvailable;
	public int final_reward;
	public int max_reward_user;

    public MissionID() { }

    public MissionID(MissionID other) {
        worldID = other.worldID;
        desc = other.desc;
        time = other.time;
        hits = other.hits;
        distance = other.distance;
        multiplierEXP = other.multiplierEXP;
        rewards = other.rewards;
        hitMode = other.hitMode;
        objectsAvailable = other.objectsAvailable;
        final_reward = other.final_reward;
        max_reward_user = other.max_reward_user;

}

	public MissionID(int newWorldID, string newDesc, int newTime, int newHits,int newDistance,int newMultiplierEXP, Dictionary <int,int> newRewards,  int newHitMode, int newObjectsAvailable,int newFinal_reward, int newMax_reward)
	{
		worldID = newWorldID;
		desc = newDesc;
		time = newTime;
		hits = newHits;
		distance = newDistance;
		multiplierEXP = newMultiplierEXP;
		rewards = newRewards;
		hitMode = newHitMode;
		objectsAvailable = newObjectsAvailable;
		final_reward = newFinal_reward;
		max_reward_user = newMax_reward;
	}

    public string toStringObjectives(bool success) {
        string InfoObjectives = string.Format("<align=\"center\">{0}</align>\n", Lean.Localization.LeanLocalization.GetTranslationText("%objectives%"));
        if (distance > 0)
        {
            InfoObjectives += string.Format("<sprite name=\"icon_distance\"> Recorre<gradient=\"Gradient_orange\"> {0}</gradient> m.", distance);
            if (success)
                InfoObjectives += " <sprite name=\"icon_check\">\n";
            else
                InfoObjectives += "\n";
        }
        if (hitMode == 1)
        {
            InfoObjectives += string.Format("<sprite name=\"icon_chicken\">Salva al menos<gradient=\"Gradient_orange\"> 1</gradient> pollo.");
            if (success)
                InfoObjectives += " <sprite name=\"icon_check\">\n";
            else
                InfoObjectives += "\n";
        }
        else if (hitMode == 2)
        {
            InfoObjectives += string.Format("<sprite name=\"icon_chicken\">Resiste máximo<gradient=\"Gradient_orange\"> {0}</gradient> golpes.", hits);
            if (success)
                InfoObjectives += " <sprite name=\"icon_check\">\n";
            else
                InfoObjectives += "\n";
        }
        if (time > 0)
        {
            InfoObjectives += string.Format("<sprite name=\"icon_clock\">Tienes<gradient=\"Gradient_orange\"> {0}</gradient> s.", time);
            if (success)
                InfoObjectives += " <sprite name=\"icon_check\">\n";
            else
                InfoObjectives += "\n";
        }

        if (objectsAvailable == 1 && !success)
        {
            InfoObjectives += "\n<align=\"center\"><size=85%><gradient=\"Gradient_green\">¡Objetos disponibles!</gradient></size></align>";
        }
        return InfoObjectives;

    }
		


}
