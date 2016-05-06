using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using TaskyPCL;


namespace TaskyAndroid.Screens 
{
	//TODO: implement proper lifecycle methods
	[Activity (Label = "Task Details")]			
	public class TaskDetailsScreen : Activity 
	{
		protected TodoItem task = new TodoItem();
		protected Button cancelDeleteButton = null;
		protected EditText notesTextEdit = null;
		protected EditText nameTextEdit = null;
		protected Button saveButton = null;
		CheckBox doneCheckbox;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Console.WriteLine("Locale: "+ Resources.Configuration.Locale); // eg. es_ES
			Console.WriteLine("Locale: "+ Resources.Configuration.Locale.DisplayName); // eg. español (España)


			View titleView = Window.FindViewById(Android.Resource.Id.Title);
			if (titleView != null) {
			  IViewParent parent = titleView.Parent;
			  if (parent != null && (parent is View)) {
			    View parentView = (View)parent;
			    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75 ,0xFF)); //38, 117 ,255
			  }
			}

			int todoId = Intent.GetIntExtra("TaskID", 0);
			if(todoId > 0) 
			{
				task = HomeScreen.TodoManager.GetTodo (todoId);
			}
			
			// set our layout to be the home screen
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
			notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			
			// find all our controls
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);

			// get translations
			var cancelString = Resources.GetText (Resource.String.taskcancel); //getResources().getText(R.string.main_title)
			var deleteString = Resources.GetText (Resource.String.taskdelete);

			// set the cancel delete based on whether or not it's an existing task
			cancelDeleteButton.Text = ( (task == null || task.ID == 0) ? cancelString : deleteString); 
			
			nameTextEdit.Text = task.Name;
			notesTextEdit.Text = task.Notes;
			doneCheckbox.Checked = task.Done;

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		protected void Save()
		{
			task.Name = nameTextEdit.Text;
			task.Notes = notesTextEdit.Text;
			task.Done = doneCheckbox.Checked;
//			HACK: 
			HomeScreen.TodoManager.SaveTodo(task);
			Finish();
		}
		
		protected void CancelDelete()
		{
			if(task.ID != 0) 
			{
				HomeScreen.TodoManager.DeleteTodo(task.ID);
			}
			Finish();
		}
	}
}