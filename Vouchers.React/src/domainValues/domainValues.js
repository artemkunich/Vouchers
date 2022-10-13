import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {getImageSrc} from '../imageSources.js'
import {ValueDetail} from '../valueDetail/valueDetail.js'
import {TransactionRequestDetail} from '../holderTransactionRequests/transactionRequestDetail.js'
import "bootstrap-icons/font/bootstrap-icons.css";

const defaultValuesFilter = {
    ticker: "",
    issuerName: "",
}

export const DomainValues = ({api}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [valuesFilter, setValuesFilter] = useState({
        ...defaultValuesFilter,
        domainId: currentDomainAccount.domainId,
    });
    const [values, setValues] = useState([])
    const [selectedValue4Detail, setSelectedValue4Detail] = useState()
    const [selectedValue4Request, setSelectedValue4Request] = useState()

    const getValuesAsync = useCallback(async (valuesQuery) => {
        const domainValues = await api.getDomainValues(token, valuesQuery)
        if(domainValues)
            setValues(domainValues)
    }, [token, currentDomainAccount]);

    const onCancel = useCallback(() => {
        setSelectedValue4Detail()
        setSelectedValue4Request()
    }, [])

    useEffect(() => {
        getValuesAsync({
            ...defaultValuesFilter,
            domainId: currentDomainAccount.domainId,
        })
    }, [token, currentDomainAccount])

    return <div className="container">
        <h1 className="header mt-2 mb-3">Domain values</h1>
        {
        selectedValue4Detail ? 
            <ValueDetail valuesItem={selectedValue4Detail} api={api} onCancel={onCancel}></ValueDetail>
        : selectedValue4Request ?
            <TransactionRequestDetail transactionRequest={{
                unitTypeId: selectedValue4Request.id,
                unitTicker: selectedValue4Request.ticker,
                unitImageId: selectedValue4Request.imageId,
                unitIssuerAccountId: selectedValue4Request.issuerAccountId,
                unitIssuerEmail: selectedValue4Request.issuerEmail,
                unitIssuerName: selectedValue4Request.issuerName,
                mustBeExchangeable: false,
                dueDate: formatDate(new Date()),
                amount: 0,
                maxDaysBeforeValidityStart: 0,
                minDaysBeforeValidityEnd: selectedValue4Request.issuerAccountId == currentDomainAccount.id ? 0 : 7,
                debtorAccountId: currentDomainAccount.id
            }} api={api} onCancel={onCancel}></TransactionRequestDetail>
        :
            <>
                <ListElement>
                    <div className="col-3 align-middle">
                        <label htmlFor="valueTickerFilter" className="form-label">Ticker</label>
                        <input type="text" id="valueTickerFilter" className="form-control" value={valuesFilter.ticker} onChange={(event) => setValuesFilter({...valuesFilter, ticker: event.target.value})}></input> 
                    </div>
                    <div className="col-3 align-middle">
                        <label htmlFor="valueIssuerFilter" className="form-label">Issuer name</label>
                        <input type="text" id="valueTickevalueIssuerFilterrFilter" className="form-control" value={valuesFilter.issuerName} onChange={(event) => setValuesFilter({...valuesFilter, issuerName: event.target.value})}></input> 
                    </div> 
                    <div className="col-2 align-middle" > 
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => getValuesAsync(valuesFilter)}>Search</button>
                    </div>
                </ListElement>
                {
                    values.map((value, i) => <Value key={value.id} valueItem={value} setSelectedValue4Detail={setSelectedValue4Detail} setSelectedValue4Request={setSelectedValue4Request}/>)
                }
            </>
        }
    </div>

}

const Value = ({valueItem, setSelectedValue4Detail, setSelectedValue4Request}) => {

    const [value, setValue] = useState(valueItem)

    return <ListElement>
        <div className="col-2 align-middle">
            { value.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(value.imageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{value.ticker}</p>
            <p className='mb-0'>{value.description}</p>
            <p className='mb-0'>Issuer: {value.issuerName} ({value.issuerEmail})</p>
        </div>
        <div className="col-2 align-middle">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => setSelectedValue4Detail(value)}>
                Detail
            </button>
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => setSelectedValue4Request(value)}>
                New request
            </button>
        </div>
    </ListElement>
}

const formatDate = (date) => {

    var month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day ].join('-');
}