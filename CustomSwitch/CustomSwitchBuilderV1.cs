using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CustomSwitch {
    /// <summary>
    /// Void Switch Builder
    /// </summary>
    /// <typeparam name="T">Can take any generic type T</typeparam>
    public class CustomSwitchBuilderV1<T> {

        private T _obj;
        public T Obj { get { return _obj; } }

        public CustomSwitchBuilderV1(T obj) {
            _obj = obj;
        }

        public IThenResult<T> Case(T caseT) {
            if(_obj.Equals(caseT)) {
                return new ThenResult<T>(true, false, _obj);
            }
            return new ThenResult<T>(false, false, _obj);
        }   
    }

    public delegate void ExecuteAction<T>(T obj);
    public interface IThenResult<T> {
        public bool IsThisCase { get; set; }
        public bool CaseExecuted { get; set; }
        public T Value { get; set; }
        public ICaseResult<T> Then(ExecuteAction<T> executeAction);
    }
    public class ThenResult<T>: IThenResult<T> {
        public bool IsThisCase { get; set; }
        public bool CaseExecuted { get; set; }
        public T Value { get; set; }

        public ThenResult(bool isThisCase,bool caseExecuted,  T value) {
            IsThisCase = isThisCase;
            CaseExecuted = caseExecuted;
            Value = value;
        }

        public ICaseResult<T> Then(ExecuteAction<T> executeAction) {
            if(IsThisCase) {
                executeAction(Value);
                CaseExecuted = true;
                return new CaseResult<T>(CaseExecuted, Value);
            }
            if(CaseExecuted) {
                return new CaseResult<T>(CaseExecuted, Value);
            }
            CaseExecuted = false;
            return new CaseResult<T>(CaseExecuted, Value);
        }
    }

    public interface ICaseResult<T> {
        public T Object { get; set; }
        public bool CaseExecuted { get; set; }
        public IThenResult<T> Case(T caseT);
        public T Default(ExecuteAction<T> executeAction);  
    }

    public class CaseResult<T>: ICaseResult<T> {
        public T Object { get; set; }
        public bool CaseExecuted { get; set; }
        public CaseResult(bool caseExecuted, T obj ) {
            CaseExecuted = caseExecuted;
            Object = obj;
        }
        public IThenResult<T> Case(T caseT) {
            if(Object.Equals(caseT)) {
                return new ThenResult<T>(true, CaseExecuted, Object);
            }
            return new ThenResult<T>(false, CaseExecuted, Object);
        }

        public T Default(ExecuteAction<T> executeAction) {
            if(CaseExecuted is not true) {
                executeAction(Object);
            }
            return Object;
        }

    }
}
