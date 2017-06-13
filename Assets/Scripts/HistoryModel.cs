using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryModel : MonoBehaviour {

	[SerializeField]
	private UnityEngine.UI.Text title;
	[SerializeField]
	private HistoryNote historyNotePrefab;
	[SerializeField]
	private Transform historyLayout;
	[SerializeField]
	private GameObject clearQuestion;
	[SerializeField]
	private GameObject pall;

	void Awake () {
		//DataTransfer.Child = new User (false, 1, "", "", "Иванов Иван Иванович", "");
		title.text = DataTransfer.Child.FIO;
		History [] history = DBWorker.loadHistory (DataTransfer.Child);
		for (int i = 0; i < history.Length; i++) {
			HistoryNote historyNote = (HistoryNote)Instantiate (historyNotePrefab, historyLayout);
			historyNote.transform.localScale = new Vector3 (1, 1, 1);
			historyNote.setData (history[i]);
		}
	}

	public void showClearQuestion ( bool show = true ) {
		clearQuestion.active = show;
		pall.active = show;
	}

	public void clearHistory () {
		DBWorker.removeHistory (DataTransfer.Child);
		HistoryNote [] historyNotes = historyLayout.GetComponentsInChildren <HistoryNote> ();
		for (int i = 0; i < historyNotes.Length; i++) {
			Destroy (historyNotes [i].gameObject);
		}
		showClearQuestion (false);
	}

	public void toChildList () {
		Application.LoadLevel (5);
	}
}
