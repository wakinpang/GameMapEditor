using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil
{
    public class MonsterControlSection : INotifyPropertyChanged {
        String _name = "";
        String _value = "";

        public String Name {
            get { return _name; }
            set {
                if (value != "") {
                    this._name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public String Value {
            get { return _value; }
            set {
                if (value != "") {
                    this._value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public MonsterControlSection(String name, String value) {
            Name = name;
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String Info) {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Info));
            }
        }
    }

    public class HistorySection : INotifyPropertyChanged {
        string _projectPath = "";

        public string ProjectPath {
            get { return _projectPath; }
            set {
                if (value != "")
                {
                    this._projectPath = value;
                    if (PropertyChanged != null) {
                        PropertyChanged(this, new PropertyChangedEventArgs("ProjectPath"));
                    }
                }
            }
        }

        public HistorySection(string path) {
            ProjectPath = path;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
