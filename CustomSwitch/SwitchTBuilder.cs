using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSwitch {
    public class SwitchTBuilder<SwitchType> {
        private SwitchType _matchValue;
        public SwitchTBuilder(SwitchType matchValue) {
            _matchValue = matchValue;
        }

        public IThenResultT<SwitchType> Case(SwitchType caseTValue) {
            if(_matchValue.Equals(caseTValue)) {
                return new ThenResultT<SwitchType>(true, _matchValue, null);
            }
            return new ThenResultT<SwitchType>(false, _matchValue, null);  
        }
    }

    public interface IThenResultT<SwitchType> {
        public ICaseResultT<SwitchType, TModel> ThenT<TModel>(Func<TModel, TModel> func);
    }

    public class ThenResultT<SwitchType>: IThenResultT<SwitchType> {
        public bool ExecuteThen { get; private set; }
        public SwitchType _matchValue { get; private set; }
        public object _modelObj { get; private set; }
        public ThenResultT(bool executeThen, SwitchType matchValue, object model) {
            ExecuteThen = executeThen;
            _matchValue=matchValue;
            _modelObj = model;
        }
        public ICaseResultT<SwitchType, TModel> ThenT<TModel>(Func<TModel, TModel> funcThen) {
            if(ExecuteThen) {
                var model = funcThen((TModel)_modelObj);
                return new CaseResultT<SwitchType, TModel>(model, _matchValue);
            } else {
                if(_modelObj is null) {
                    return new CaseResultT<SwitchType, TModel>(null, _matchValue);
                }
                return new CaseResultT<SwitchType, TModel>(_modelObj, _matchValue);
            }
        }
    }

    public interface ICaseResultT<SwitchType, TModel> {
        public IThenResultT<SwitchType> Case(SwitchType caseTValue);
        public IBuildResultT<TModel> DefaultT<DefT>(Func<DefT, DefT> func);
    }

    public class CaseResultT<SwitchType, TModel>: ICaseResultT<SwitchType, TModel> {
        public SwitchType _matchValue { get; private set;}
        public object ModelObj { get; private set; }    
        public CaseResultT(object modelObj, SwitchType matchValue) {
            ModelObj = modelObj;
            _matchValue = matchValue;
        }
        public IThenResultT<SwitchType> Case(SwitchType caseTValue) {
            if(_matchValue.Equals(caseTValue)) {
                return new ThenResultT<SwitchType>(true, _matchValue, ModelObj);
            }
            return new ThenResultT<SwitchType>(false, _matchValue, ModelObj);
        }

        public IBuildResultT<TModel> DefaultT<DefT>(Func<DefT, DefT> func) {
            if(ModelObj is null) {
                var model = func((DefT)ModelObj);
                return new BuildResultT<TModel>(model);
            }
            return new BuildResultT<TModel>(ModelObj);
        }
    }

    public interface IBuildResultT<TModel> {
        public object Build();
        public TModel BuildT();
    }

    public class BuildResultT<TModel> : IBuildResultT<TModel> {
        private object _model;
        public BuildResultT(object model) {
            _model = model;
        }

        public object Build() {
            return _model;
        }

        public TModel BuildT() {
            return (TModel)_model;
        }
    }
}
