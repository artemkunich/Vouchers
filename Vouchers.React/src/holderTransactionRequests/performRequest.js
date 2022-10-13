import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import {useSelector, useDispatch} from 'react-redux'
import {ListElement} from '../list/listElement.js'
import {TransactionRequestDetail} from './transactionRequestDetail.js'
import "bootstrap-icons/font/bootstrap-icons.css";


export const PerformRequest = ({api}) => {
    
    const token = useSelector(state => state.user.token)

    const [transactionRequestId, setTransactionRequestId] = useState();
    const [selectedTransactionRequestDetail, setSelectedTransactionRequestDetail] = useState();

    const onCancel = useCallback(() => {
        setSelectedTransactionRequestDetail()
    }, [])

    const loadTransactionRequestAsync = useCallback(async (transactionRequestId) => {
        const result = await api.getTransactionRequest(token, transactionRequestId)
        if(result){
            setSelectedTransactionRequestDetail(result)
            return
        }
            
        alert("Transaction request was not found")
    }, [token, api])

    return <div className="container">
        <h1 className="header mt-2 mb-3">Perform transaction request</h1>
        {
            selectedTransactionRequestDetail ? <TransactionRequestDetail transactionRequest={selectedTransactionRequestDetail} api={api} onCancel={onCancel}/>
            :
            <ListElement>
                <div className="col-3 align-middle">
                    <label htmlFor="transactionRequestId" className="form-label">Request id</label>
                    <input type="text" id="transactionRequestId" className="form-control" value={transactionRequestId} onChange={(event) => setTransactionRequestId(event.target.value)}></input> 
                </div> 
                <div className="col-1 d-grid mb-2" >
                    <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await loadTransactionRequestAsync(transactionRequestId)}>Find</button>
                </div> 
            </ListElement>   
        }

    </div>
}