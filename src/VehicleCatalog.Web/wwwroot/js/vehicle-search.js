(function() {
    'use strict';

    const makeSelect = document.getElementById('makeSelect');
    const yearInput = document.getElementById('yearInput');
    const searchBtn = document.getElementById('searchBtn');
    const loadingIndicator = document.getElementById('loadingIndicator');
    const resultsContainer = document.getElementById('resultsContainer');
    const vehicleTypesSection = document.getElementById('vehicleTypesSection');
    const vehicleModelsSection = document.getElementById('vehicleModelsSection');
    const vehicleTypesList = document.getElementById('vehicleTypesList');
    const vehicleModelsList = document.getElementById('vehicleModelsList');
    const errorContainer = document.getElementById('errorContainer');
    const errorMessage = document.getElementById('errorMessage');
    const noResultsContainer = document.getElementById('noResultsContainer');

    let currentMakeId = null;
    let currentYear = null;


    searchBtn.addEventListener('click', handleSearch);
    yearInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            handleSearch();
        }
    });

    async function handleSearch() {
        currentMakeId = parseInt(makeSelect.value);
        currentYear = parseInt(yearInput.value);

       if (!currentMakeId) {
            showError('Please select a vehicle make');
            return;
        }

        if (!currentYear || currentYear < 1900 || currentYear > new Date().getFullYear() + 2) {
            showError('Please enter a valid year between 1900 and ' + (new Date().getFullYear() + 2));
            return;
        }

        hideAllSections();
        showLoading();

        try {
            const types = await fetchVehicleTypes(currentMakeId);
           
            const models = await fetchModels(currentMakeId, currentYear);

            hideLoading();
            
            if (types && types.length > 0) {
                displayVehicleTypes(types);
            }

            if (models && models.length > 0) {
                displayModels(models);
            }

            if ((!types || types.length === 0) && (!models || models.length === 0)) {
                showNoResults();
            } else {
                resultsContainer.style.display = 'block';
                resultsContainer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            }

        } catch (error) {
            hideLoading();
            showError('Failed to retrieve vehicle data. Please try again.');
            console.error('Search error:', error);
        }
    }

    async function fetchVehicleTypes(makeId) {
        const response = await fetch(`/Home/GetVehicleTypes?makeId=${makeId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch vehicle types');
        }
        return await response.json();
    }

    async function fetchModels(makeId, year) {
        const response = await fetch(`/Home/GetModels?makeId=${makeId}&year=${year}`);
        if (!response.ok) {
            throw new Error('Failed to fetch models');
        }
        return await response.json();
    }

    function displayVehicleTypes(types) {
        vehicleTypesList.innerHTML = '';
        
        types.forEach((type, index) => {
            const card = document.createElement('div');
            card.className = 'type-card';
            card.style.animationDelay = `${index * 0.05}s`;
            card.innerHTML = `
                <div class="type-icon">🏷️</div>
                <div class="type-name">${escapeHtml(type.vehicleTypeName)}</div>
            `;
            vehicleTypesList.appendChild(card);
        });

        vehicleTypesSection.style.display = 'block';
    }

    function displayModels(models) {
        vehicleModelsList.innerHTML = '';
        
        models.forEach((model, index) => {
            const card = document.createElement('div');
            card.className = 'model-card';
            card.style.animationDelay = `${index * 0.03}s`;
            card.innerHTML = `
                <div class="model-header">
                    <div class="model-icon">🚙</div>
                    <div class="model-year">${model.modelYear}</div>
                </div>
                <div class="model-name">${escapeHtml(model.modelName)}</div>
            `;
            vehicleModelsList.appendChild(card);
        });

        vehicleModelsSection.style.display = 'block';
    }

    function showLoading() {
        loadingIndicator.style.display = 'flex';
    }

    function hideLoading() {
        loadingIndicator.style.display = 'none';
    }

    function showError(message) {
        errorMessage.textContent = message;
        errorContainer.style.display = 'block';
        setTimeout(() => {
            errorContainer.style.display = 'none';
        }, 5000);
    }

    function showNoResults() {
        noResultsContainer.style.display = 'block';
    }

    function hideAllSections() {
        resultsContainer.style.display = 'none';
        vehicleTypesSection.style.display = 'none';
        vehicleModelsSection.style.display = 'none';
        errorContainer.style.display = 'none';
        noResultsContainer.style.display = 'none';
    }

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

})();
