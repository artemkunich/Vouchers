import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {DomainAccounts} from '../domainAccounts/domainAccounts.js'
import {getImageSrc} from '../imageSources.js'
import {HolderTransactionDetail} from '../holderTransactionDetail/holderTransactionDetail.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const TransactionRequestDetail = ({transactionRequest, api, onCancel}) => {

    const token = useSelector(state => state.user.token)
    const currentDomainAccount = useSelector(state => state.domainAccount.currentAccount)

    const [isSelectCreditorState, setIsSelectCreditorState] = useState(false)
    const [isPerformState, setIsPerformState] = useState(false)

    const [transactionRequestDetail, setTransactionRequestDetail] = useState({
        ...transactionRequest,
        dueDate: transactionRequest.dueDate?.substring(0,10)
    })

    useEffect(() => {
        if(!transactionRequest.debtorAccountId) {
            transactionRequestDetail.debtorAccountId = currentDomainAccount.id
        }
    }, [token, currentDomainAccount])

    const setSelectedCreditor = useCallback((creditorAccount) => {
        setIsSelectCreditorState(false)

        setTransactionRequestDetail({
            ...transactionRequestDetail,
            creditorAccountId: creditorAccount?.id,
            counterpartyName: creditorAccount?.name,
            counterpartyEmail: creditorAccount?.email,
            counterpartyImageId: creditorAccount?.imageId           
        })
    }, [transactionRequestDetail])

    const saveRequestAsync = useCallback(async (request) => {
        const requestId = await api.postTransactionRequest(token, request)
        if(requestId){
            setTransactionRequestDetail({
                ...transactionRequestDetail,
                id: requestId
            })
        }
    }, [token, api, transactionRequestDetail])

    console.log(transactionRequestDetail.unitIssuerAccountId + " " + currentDomainAccount.id)

    return <div className="container"> {
        isSelectCreditorState ?
            <DomainAccounts setSelectedDomainAccount={setSelectedCreditor} onCancel={() => setIsSelectCreditorState(false)} api={api}/> 
        :
            isPerformState ? 
                <HolderTransactionDetail transaction={{
                    creditorAccountId: transactionRequestDetail?.creditorAccountId ?? currentDomainAccount.id,
                    debtorAccountId: transactionRequestDetail.debtorAccountId,
                    debtorName: transactionRequestDetail.counterpartyName,
                    debtorEmail: transactionRequestDetail.counterpartyEmail,
                    amount: 0,
                    unitTypeId: transactionRequestDetail.unitTypeId,
                    unitIssuerAccountId: transactionRequestDetail.unitIssuerAccountId,
                    unitTicker: transactionRequestDetail.unitTicker,
                    items: [],
                    message: ""
                }} transactionRequest={transactionRequestDetail} api={api} onCancel={onCancel}/>
            :
                <>
                    <div className="row mb-2">
                        <h3>{transactionRequestDetail.id && `Request id: ${transactionRequestDetail.id}`}</h3>
                    </div>
                    <div className="row mb-2">
                        <div className="col-2 align-middle">
                            {transactionRequestDetail.unitImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequestDetail.unitImageId)}></img> }
                            <p className='mb-0'>Ticker: {transactionRequestDetail.unitTicker}</p>
                            <p>Issuer: {transactionRequestDetail.unitIssuerName} {transactionRequestDetail.unitIssuerEmail && `(${transactionRequestDetail.unitIssuerEmail})`}</p>
                            <div className="align-middle form-check">
                                <label htmlFor="mustBeExchangeable" className="form-check-label">Must be exchangeable</label>
                                <input type="checkbox" id="mustBeExchangeable" className="form-check-input" checked={transactionRequestDetail.mustBeExchangeable} 
                                    onChange={() => setTransactionRequestDetail({...transactionRequestDetail, mustBeExchangeable: !transactionRequestDetail.mustBeExchangeable})} 
                                    disabled={transactionRequestDetail.id || transactionRequestDetail.unitIssuerAccountId == currentDomainAccount.id}>
                                </input>
                            </div>
                        </div>
                        <div className="col-2 align-middle">
                            { transactionRequestDetail.counterpartyImageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(transactionRequestDetail.counterpartyImageId)}></img> }
                            <p>Counterparty: {transactionRequestDetail.counterpartyName} {transactionRequestDetail.counterpartyEmail && `(${transactionRequestDetail.counterpartyEmail})`}</p>
                            
                            {
                                !transactionRequestDetail.id &&
                                <div className="d-grid" > 
                                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white'}} onClick={() => setIsSelectCreditorState(true)}>Select creditor</button>
                                </div>
                            }
                        </div>
                    </div>

                    <div className="row mb-2">
                        <div className="col-3">
                            <label htmlFor="amount" className="form-label">Amount</label>
                            <input type="number" id="amount" className="form-control" value={transactionRequestDetail.amount} onChange={(event) => setTransactionRequestDetail({...transactionRequestDetail, amount: event.target.value})} disabled={transactionRequestDetail.id}></input>                               
                        </div> 
                        <div className="col-3">
                            <label htmlFor="dueDate" className="form-label">Due date</label>
                            <input type="date" id="dueDate" className="form-control" value={transactionRequestDetail.dueDate} onChange={(event) => setTransactionRequestDetail({...transactionRequestDetail, dueDate: event.target.value})} disabled={transactionRequestDetail.id}></input>               
                        </div>
                        <div className="col-3">
                            <label htmlFor="maxDaysBeforeValidityStart" className="form-label">Max days before validity start</label>
                            <input type="text" id="maxDaysBeforeValidityStart" className="form-control" 
                                value={transactionRequestDetail.maxDaysBeforeValidityStart} 
                                onChange={(event) => setTransactionRequestDetail({...transactionRequestDetail, maxDaysBeforeValidityStart: event.target.value})}
                                disabled={transactionRequestDetail.id || transactionRequestDetail.unitIssuerAccountId == currentDomainAccount.id}>
                            </input>                
                        </div>
                        <div className="col-3">
                            <label htmlFor="minDaysBeforeValidityEnd" className="form-label">Min days before validity end</label>
                            <input type="text" id="minDaysBeforeValidityEnd" className="form-control" 
                                value={transactionRequestDetail.minDaysBeforeValidityEnd}
                                onChange={(event) => setTransactionRequestDetail({...transactionRequestDetail, minDaysBeforeValidityEnd: event.target.value})}
                                disabled={transactionRequestDetail.id ||transactionRequestDetail.unitIssuerAccountId == currentDomainAccount.id}></input>                
                        </div> 
                    </div>
                    <div className="row mb-2">
                        <div className="col-12">
                            <label  htmlFor="message" className="form-label">Message</label>
                            <textarea id="message" className="form-control" rows="auto" maxLength="1024" value={transactionRequestDetail.message} onChange={(event) => setTransactionRequestDetail({ ...transactionRequestDetail, message: event.target.value })} disabled={transactionRequestDetail.id}></textarea>  
                        </div>
                    </div>
                    <div className="row mb-2">
                        {
                            !transactionRequestDetail.transactionId && transactionRequestDetail.debtorAccountId != currentDomainAccount.id && 
                            <div className="col-1 d-grid mb-2" > 
                                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} title={"Perform"} onClick={() => setIsPerformState(true)}>
                                    Perform
                                </button>
                            </div>
                        }
                        {
                            !transactionRequestDetail.id ?
                            <div className="col-1 d-grid mb-2" > 
                                <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await saveRequestAsync({...transactionRequestDetail, unitImageId: undefined, counterpartyImageId: undefined})}>Save</button>
                            </div> : <></>
                        }
                        <div className="col-1 d-grid mb-2" > 
                            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={() => onCancel()}>Cancel</button>
                        </div>
                    </div>
                </>
    }
    </div>
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