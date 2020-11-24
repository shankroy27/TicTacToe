using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manucontroller : MonoBehaviour {
	enum dif
	{
		easy=1,medium,hard
	}
	public GameObject cmainmenu, cdifficulty, cabout;
	void Awake()
	{
		cdifficulty.SetActive (false);
		cabout.SetActive (false);
	}
	public void sp ()
	{
		cmainmenu.SetActive (false);
		cabout.SetActive (false);
		cdifficulty.SetActive (true);
	}
	public void tp()
	{
		SceneManager.LoadScene ("namemenu", LoadSceneMode.Single);
	}
	public void credits ()
	{
		cmainmenu.SetActive (false);
		cabout.SetActive (true);
		cdifficulty.SetActive (false);
	}
	public void ex()
	{
		Application.Quit ();
	}
	public void back()
	{
		cdifficulty.SetActive (false);
		cmainmenu.SetActive (true);
		cabout.SetActive (false);
	}
	public void easy ()
	{
		data.diff = (int)dif.easy;
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}
	public void medium ()
	{
		data.diff = (int)dif.medium;
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}
	public void veteran ()
	{
		data.diff = (int)dif.hard;
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}
}
