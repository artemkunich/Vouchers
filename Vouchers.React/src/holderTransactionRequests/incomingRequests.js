import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {TransactionRequestDetail} from './transactionRequestDetail.js'
import {HolderTransactionDetail} from '../holderTransactionDetail/holderTransactionDetail.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css"

export const IncomingRequests = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionRequestsFilter, setTransactionRequestsFilter] = useState({
        ticker: "",
        issuerName: "",
        includeIncoming: true,
        includeOutgoing: false,
        includePaid: false,
        includeUnpaid: true
    });
    const [transactionRequests, setTransactionRequests] = useState([]);
    const [selectedTransactionRequestDetail, setSelectedTransactionRequestDetail] = useState();
    const [selectedTransactionRequest2Perform, setSelectedTransactionRequest2Perform] = useState();

    const loadTransactionRequestsAsync = useCallback(async (transactionRequestsQuery) => {
        const transactionRequests = await api.getTransactionRequests(token, transactionRequestsQuery)
        if(transactionRequests)
            setTransactionRequests(transactionRequests);
    }, [token])

    const onCancel = useCallback(() => {
        setSelectedTransactionRequestDetail()
        setSelectedTransactionRequest2Perform()
    }, [])

    useEffect(() => {
        const effectAsync = async () => {
            await loadTransactionRequestsAsync(transactionRequestsFilter);
        } 

        effectAsync();
    }, [token, currentDomainAccount])

    return <div className="container">
        <h1 className="header mt-2 mb-3">Incoming transaction requests</h1>
        {
            selectedTransactionRequestDetail ? 
            <TransactionRequestDetail transactionRequest={selectedTransactionRequestDetail} api={api} onCancel={onCancel}/>
            : 
            selectedTransactionRequest2Perform ? <HolderTransactionDetail transaction={{
                creditorAccountId: selectedTransactionRequest2Perform?.creditorAccountId ?? currentDomainAccount.id,
                debtorAccountId: selectedTransactionRequest2Perform.debtorAccountId,
                debtorAccountName: selectedTransactionRequest2Perform.counterpartyName,
                debtorAccountEmail: selectedTransactionRequest2Perform.counterpartyEmail,
                quantity: 0,
                voucherValueId: selectedTransactionRequest2Perform.unitTypeId,
                voucherValueIssuerAccountId: selectedTransactionRequest2Perform.unitIssuerAccountId,
                voucherValueTicker: selectedTransactionRequest2Perform.unitTicker,
                items: [],
                message: ""
            }} transactionRequest={selectedTransactionRequest2Perform} api={api} onCancel={onCancel}/>
            :
            <>
                <ListElement>
                    <div className="col-3 align-middle">
                        <label htmlFor="transactionRequestTickerFilter" className="form-label">Ticker</label>
                        <input type="text" id="transactionRequestTickerFilter" className="form-control" value={transactionRequestsFilter.ticker} onChange={(event) => setTransactionRequestsFilter({...transactionRequestsFilter, ticker: event.target.value})}></input> 
                    </div>
                    <div className="col-3 align-middle">
                        <label htmlFor="transactionRequestIssuerNameFilter" className="form-label">Issuer name</label>
                        <input type="text" id="transactionRequestIssuerNameFilter" className="form-control" value={transactionRequestsFilter.issuerName} onChange={(event) => setTransactionRequestsFilter({...transactionRequestsFilter, issuerName: event.target.value})}></input> 
                    </div>  
                    <div className="col-3 align-middle">
                        <div className='container'>
                            <div className='row'>
                                <div className="col-12 align-middle form-check">
                                    <label htmlFor="includePaid" className="form-check-label">Include paid</label>
                                    <input type="checkbox" id="includePaid" className="form-check-input" checked={transactionRequestsFilter.includePaid} 
                                        onChange={() => setTransactionRequestsFilter({...transactionRequestsFilter, includePaid: !transactionRequestsFilter.includePaid})} >
                                    </input> 
                                </div>
                                <div className="col-12 align-middle form-check">
                                    <label htmlFor="includeUnpaid" className="form-check-label">Include unpaid</label>
                                    <input type="checkbox" id="includeUnpaid" className="form-check-input" checked={transactionRequestsFilter.includeUnpaid} 
                                        onChange={() => setTransactionRequestsFilter({...transactionRequestsFilter, includeUnpaid: !transactionRequestsFilter.includeUnpaid})} >
                                    </input>
                                </div>
                            </div>
                            
                        </div>
                    </div> 

                    <div className="col-1 d-grid mb-2" >
                        <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await loadTransactionRequestsAsync(transactionRequestsFilter)}>Search</button>
                    </div> 
                </ListElement>   
                {              
                    transactionRequests.map((transactionRequest, i) => <TransactionRequest key={transactionRequest.id} transactionRequest={transactionRequest} setSelectedTransactionRequestDetail={setSelectedTransactionRequestDetail} setSelectedTransactionRequest2Perform={setSelectedTransactionRequest2Perform}/>)
                }
            </>
        }

    </div>
}

const TransactionRequest = ({transactionRequest, setSelectedTransactionRequestDetail, setSelectedTransactionRequest2Perform}) => {
    return <ListElement>
        <div className="col-2 align-middle">
            { transactionRequest.unitImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequest.unitImageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{transactionRequest.unitTicker}</p>
            <p className='mb-0'>Issuer: {transactionRequest.unitIssuerName} ({transactionRequest.unitIssuerEmail})</p>
        </div> 
        <div className=" col-2">         
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Detail"} onClick={() => setSelectedTransactionRequestDetail(transactionRequest)}>
                Detail
            </button>
            
            {
                !transactionRequest.transactionId && <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Perform"} onClick={() => setSelectedTransactionRequest2Perform(transactionRequest)}>
                    Perform
                </button>
            }
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