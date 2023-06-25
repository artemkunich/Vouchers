import React, { useState } from 'react'
import { ListElement } from '../common/list/list.tsx'
import "bootstrap-icons/font/bootstrap-icons.css";


export const PerformRequestForm = ({loadTransactionRequestAsync}) => {

    const [transactionRequestId, setTransactionRequestId] = useState()

    return <ListElement>
        <div className="col-3 align-middle">
            <label htmlFor="transactionRequestId" className="form-label">Request id</label>
            <input type="text" id="transactionRequestId" className="form-control" value={transactionRequestId} onChange={(event) => setTransactionRequestId(event.target.value)}></input> 
        </div> 
        <div className="col-1 d-grid mb-2">
            <button className="btn" style={{backgroundColor: '#59a8fc', color: 'white', marginTop: 32}} onClick={async () => await loadTransactionRequestAsync(transactionRequestId)}>Find</button>
        </div> 
    </ListElement>
}