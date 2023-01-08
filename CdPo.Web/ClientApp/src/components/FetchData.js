import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { printForms: [], loading: true };
  }

  componentDidMount() {
    this.populatePrintFormsData();
  }

  static renderPrintFormsTable(printForms) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Название печатной формы</th>
          </tr>
        </thead>
        <tbody>
          {printForms.map(printForm =>
            <tr key={printForm.id}>
              <td>{printForm.name}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderPrintFormsTable(this.state.printForms);

    return (
      <div>
        <h1 id="tabelLabel">Существующие печатные формы</h1>
        {contents}
      </div>
    );
  }

  async populatePrintFormsData() {
    const response = await fetch('api/test');
    const data = await response.json();
    this.setState({ printForms: data, loading: false });
  }
}
