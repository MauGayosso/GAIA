﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;


namespace FileExplorer
{
	public class Node : INotifyPropertyChanged
	{
		public Node()
		{
			children = new ObservableCollection<Node>();
			isChecked = false;
		}

		//static vars to hold current counts
		public static int files = 0;
		public static int folders = 0;
		public static int selectedFolders = 0;
		public static int selectedFiles = 0;
		public static string selectedBytes = "";

		//static vars to hold relative file paths for icons
		public static Uri fileIcon = new Uri("image/files.png", UriKind.Relative);
		public static Uri folderIcon = new Uri("image/fil.png", UriKind.Relative);
		public static Uri driveIcon = new Uri("image/drive.png", UriKind.Relative);
		public static Uri loadingIcon = new Uri("image/loading.png", UriKind.Relative);
		public static Uri pathFile = new Uri("file.txt", UriKind.Relative);

		//class members
		public ObservableCollection<Node> children { get; set; }
		public Node parent { get; set; }
		public long byteSize { get; set; }
		public string name { get; set; }
		public Uri iconLoc { get; set; }
		public string fullPath { get; set; }
		public string nodeVal { get; set; }
		public bool isFile { get; set; }
		private bool? prevVal { get; set; }
		//stores whether current node has been entered already during current recursive call
		private bool reentrancyCheck = false;

		//binding for checkbox of current node
		private bool? _isChecked { get; set; }
		public bool? isChecked
		{
			get
			{
				return _isChecked;
			}
			set
			{
				if (isChecked != value)
				{
					//if node has been etnered already than exit
					if (reentrancyCheck)
						return;
					//note that node has been entered
					reentrancyCheck = true;
					//store previous value
					prevVal = _isChecked;
					_isChecked = value;
					//UpdateCheckState();
					updateCounts();
					UpdateChildrenCheckState();
					//notify prop changed
					OnPropertyChanged("IsChecked");
					//reset entrancy for next recursive call
					reentrancyCheck = false;
				}
			}
		}

		private void updateCounts()
		{
			selectedBytes = nodeVal;
		}

		private void UpdateChildrenCheckState()
		{
			//recursively update children by calling IsChecked {set;}
			foreach (var c in children)
				if (isChecked != null)
					c.isChecked = isChecked;
		}

		//determine proper state of a parent node
		private bool? DetermineCheckState()
		{
			//if all children checked parent is checked
			bool allChildrenChecked = children.Count(c => c.isChecked == true) == children.Count;
			if (allChildrenChecked)
				return true;

			//if all children unchecked parent is unchecked
			bool allChildrenUnchecked = children.Count(c => c.isChecked == false) == children.Count;
			if (allChildrenUnchecked)
				return false;

			return null;
		}
		//reset all counts to 0
		public static void resetCounts()
		{
			files = 0;
			folders = 0;
			selectedFiles = 0;
			selectedFolders = 0;
			selectedBytes = null;
		}

		//recurively parse a Node which contains the directory at hand
		public Node DirParse()
		{
			try
			{
				//add each child directory to children, increase folder count
				foreach (string d in Directory.GetDirectories(this.fullPath))
				{
					DirectoryInfo dirInfo = new DirectoryInfo(d);
					Node curDNode = new Node()
					{
						parent = this,
						name = dirInfo.Name,
						fullPath = dirInfo.FullName,
						byteSize = 0,
						iconLoc = folderIcon,
						isFile = false,
						nodeVal = dirInfo.FullName

					};

					this.children.Add(curDNode);
					++folders;
					//recursive call for each directory found within current dir
					curDNode.DirParse();
				}

				//add each child file to children, increase file count
				foreach (string f in Directory.GetFiles(this.fullPath))
				{
					FileInfo fileInfo = new FileInfo(f);
					Node curFNode = new Node()
					{
						parent = this,
						name = fileInfo.Name,
						fullPath = fileInfo.FullName,
						byteSize = fileInfo.Length,
						iconLoc = fileIcon,
						isFile = true,
						nodeVal = fileInfo.FullName
					};
					this.children.Add(curFNode);
					++files;
				}

				return this;
			}
			catch (Exception err)
			{
			}
			return null;
		}

		void OnPropertyChanged(string prop)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}