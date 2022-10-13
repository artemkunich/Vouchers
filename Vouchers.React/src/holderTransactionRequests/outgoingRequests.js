import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {TransactionRequestDetail} from './transactionRequestDetail.js'
import {getImageSrc} from '../imageSources.js'
import "bootstrap-icons/font/bootstrap-icons.css";


export const OutgoingRequests = ({api}) => {
    
    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [transactionRequestsFilter, setTransactionRequestsFilter] = useState({
        ticker: "",
        issuerName: "",
        includeIncoming: false,
        includeOutgoing: true,
        includePaid: false,
        includeUnpaid: true
    });
    const [transactionRequests, setTransactionRequests] = useState([]);
    const [selectedTransactionRequest, setSelectedTransactionRequest] = useState();

    const loadTransactionRequestsAsync = useCallback(async (transactionRequestsQuery) => {
        const transactionRequests = await api.getTransactionRequests(token, transactionRequestsQuery)
        if(transactionRequests)
            setTransactionRequests(transactionRequests);
    }, [api, token])

    const deleteTransactionRequestAsync = useCallback(async (transactionRequestId) => {
        await api.deleteTransactionRequest(token, transactionRequestId)
        await loadTransactionRequestsAsync(transactionRequestsFilter)
    }, [api, token, transactionRequestsFilter])

    const onCancel = useCallback(() => {
        setSelectedTransactionRequest()
    }, [])

    useEffect(() => {
        const effectAsync = async () => {
            await loadTransactionRequestsAsync(transactionRequestsFilter);
        } 

        effectAsync();
    }, [token, currentDomainAccount])

    return <div className="container">
        <h1 className="header mt-2 mb-3">Outgoing transaction requests</h1>
        {
            selectedTransactionRequest ? 
            <TransactionRequestDetail transactionRequest={selectedTransactionRequest} api={api} onCancel={onCancel}/>
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
                    transactionRequests.map((transactionRequest, i) => <TransactionRequest key={transactionRequest.id} transactionRequest={transactionRequest} setSelectedTransactionRequest={setSelectedTransactionRequest} deleteTransactionRequest={deleteTransactionRequestAsync}/>)
                }
            </>
        }

    </div>
}

const TransactionRequest = ({transactionRequest, setSelectedTransactionRequest, deleteTransactionRequest}) => {
    return <ListElement>
        <div className="col-2 align-middle">
            { transactionRequest.unitImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequest.unitImageId)}></img> }
        </div>
        <div className="col-8 align-middle">
            <p className="fs-5">{transactionRequest.unitTicker}</p>
            <p className='mb-0'>Issuer: {transactionRequest.unitIssuerName} ({transactionRequest.unitIssuerEmail})</p>
        </div> 
        <div className=" col-2">
            <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Perform"} onClick={() => setSelectedTransactionRequest(transactionRequest)}>
                Detail
            </button>
            {
                !transactionRequest.transactionId &&
                <button className="btn mt-2" style={{backgroundColor: '#59a8fc', color: 'white', marginLeft: -15, width: 125}} title={"Perform"} onClick={() => deleteTransactionRequest(transactionRequest.id)}>
                    Remove
                </button>
            }
        </div>

    </ListElement>
}

