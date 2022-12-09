import useFetchCustomers from "../hooks/CustomerHooks";
import {Customer} from "../types/customer";
import ApiStatus from "../common/apiStatus";

const CustomersList = () => {
    const { data, status, isSuccess } = useFetchCustomers();
    if (!isSuccess) return <ApiStatus status={status} />;
    return (
        <div>
            <div className="row mb-2">
                <h5 className="themeFontColor text-center">
                    List of Customers
                </h5>
            </div>
            <table className="table table-hover">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>CustomerId</th>
                </tr>
                </thead>
                <tbody>
                {data &&
                    data.map((h: Customer) => (
                        <tr key={h.id} >
                            <td>{h.id}</td>
                            <td>{h.name}</td>
                            <td>{h.customerId}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            </div>
    );


}
export default CustomersList;